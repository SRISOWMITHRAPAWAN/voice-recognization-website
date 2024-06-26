using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shared25.Models;

namespace Shared25.Service
{
   public class MstService
    {
        ConnectionString connection = new ConnectionString();
        //SqlConnection con = null;




        public MstListResponse GetAllMstDetails()
        {
            MstListResponse mstListResponse = new MstListResponse();

            try
            {
                List<MstDetailsEntity>? mstDetails = new List<MstDetailsEntity>();

                using (SqlConnection con = new SqlConnection(connection.MstConnection()))
                {
                    con.Open();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter("GetMstDetails", con);
                    dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    mstDetails = JsonConvert.DeserializeObject<List<MstDetailsEntity>>(JsonConvert.SerializeObject(dataTable));

                    con.Close();
                }

                mstListResponse.status = true;
                mstListResponse.mstDetailsEntities = mstDetails;

            }
            catch (Exception ex)
            {
                mstListResponse.status = false;
                mstListResponse.Message = "Error: " + ex.Message;
                mstListResponse.Message1 = "something is wrong with getting details";
            }

            return mstListResponse;
        }

        public MstListResponse AddMstMapping(MstMappingEntity MstM)
        {
            MstListResponse mstListResponse = new MstListResponse();
            List<MstMappingEntity> mstMappingDetail = new List<MstMappingEntity>();

            try
            {
                using (SqlConnection con = new SqlConnection(connection.MstConnection()))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("AddNotificationRoleMapping", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PSId", MstM.PSId);
                    cmd.Parameters.AddWithValue("@NotificationID", MstM.NotificationID);
                    cmd.Parameters.AddWithValue("@RoleID", MstM.RoleID);

                    // Add output parameters
                    cmd.Parameters.Add("@SUCCESS", SqlDbType.VarChar, 1).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Result", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    // Retrieve output parameter values
                    string? successFlag = cmd.Parameters["@SUCCESS"].Value.ToString();
                    string? resultMessage = cmd.Parameters["@Result"].Value.ToString();

                    if (successFlag == "1")
                    {
                        mstListResponse.status = true;
                        mstListResponse.MstMappingEntities = mstMappingDetail;
                        mstListResponse.Message = resultMessage; // Success message
                    }
                    else
                    {
                        mstListResponse.status = false;
                        mstListResponse.Message = resultMessage; // Failure message
                    }
                }
            }
            catch (Exception ex)
            {
                mstListResponse.status = false;
                mstListResponse.Message = "Something Went Wrong with adding roles";
                Console.Write(ex.ToString());
            }

            return mstListResponse;
        }

    }
}

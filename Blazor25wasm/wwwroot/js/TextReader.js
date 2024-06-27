export function readText(text,dotnetref) {
    const synth = window.speechSynthesis;
    const utterance = new SpeechSynthesisUtterance(text);
    synth.speak(utterance);
}
export function speechRecognition(DotNet) {
    var recognition = new (window.SpeechRecognition || window.webkitSpeechRecognition)();
    recognition.lang = 'en-US';
    recognition.interimResults = false;
    recognition.maxAlternatives = 1;

    recognition.onresult = function (event) {
        var speechResult = event.results[0][0].transcript;
        DotNet.invokeMethodAsync('VoiceSearchApp', speechResult);
    };

    recognition.onerror = function (event) {
        console.error(event.error);
    };

    recognition.start();

}

export function formsubmit(article) {
    var data = JSON.parse(article)
    console.log(data);
    storeInIndexedDB("TrackYourPaper", "TYP", data);
}
function storeInIndexedDB(databaseName, storeName, data) {
    try {
        const indexedDB =
            window.indexedDB ||
            window.mozIndexedDB ||
            window.webkitIndexedDB ||
            window.msIndexedDB ||
            window.shimIndexedDB;

        if (!indexedDB) {
            console.log('IndexedDB could not be found in this browser.');
        }
        if (localStorage.getItem('version') == null) {
            localStorage.setItem('version', 1);
        }
        var version = localStorage.getItem('version');
        const request = indexedDB.open(databaseName, +version);
        request.onerror = function (event) {
            console.error('An error occurred with IndexedDB');
            console.error(event);
        };

        request.onupgradeneeded = function () {
            const db = request.result;
            if (!db.objectStoreNames.contains(storeName)) {
                const store = db.createObjectStore(storeName, { keyPath: "id", autoIncrement: true });
                store.createIndex('ArticleId', ['ArticleId'], { unique: true });
                store.createIndex('ArticleName', ['ArticleName'], { unique: false });
            }
        };

        request.onsuccess = function () {
            console.log('Database opened successfully');
            localStorage.setItem('version', +version + 1);
            const db = request.result;

            const transaction = db.transaction([storeName], 'readwrite');
            const store = transaction.objectStore(storeName);
            store.add({
                ArticleId: data.ArticleId,
                ArticleName: data.ArticleName
            });
            transaction.oncomplete = function () {
                db.close();
            };
        };
    } catch (error) {
        console.log("error " + error);
    }
}
//export async function GetArticleByName(databaseName, storeName, names,dotnetref) {
//    try {
//        console.log(names);
//        var name = names.toUpperCase();
//        const indexedDB =
//            window.indexedDB ||
//            window.mozIndexedDB ||
//            window.webkitIndexedDB ||
//            window.msIndexedDB ||
//            window.shimIndexedDB;

//        if (!indexedDB) {
//            console.log('IndexedDB could not be found in this browser.');
//        }
//        var version = localStorage.getItem('version');
//        let request = window.indexedDB.open(databaseName, version);
//        return new Promise((resolve, reject) => {
//            request.onsuccess = function () {
//                console.log('Database opened successfully');
//                const db = request.result;

//                const transaction = db.transaction(storeName, 'readwrite');
//                const store = transaction.objectStore(storeName);
//                console.log(store)
//                const employeeindex = store.index('ArticleId');
//                console.log(employeeindex);
//                console.log(name);
//                const employeeQuery = employeeindex.get([name]);

//                employeeQuery.onsuccess = function () {
//                    console.log('EmailQuery', employeeQuery);
//                    var query = employeeQuery.result;
//                    if (query) {
//                        resolve(query);
//                    } else {
//                        console.log('Employee not found');
//                        reject('Employee not found');
//                    }
//                };
//            };
//        });
//    } catch (error) {
//        console.log("error " + error);
//    }
//}

export async function GetArticleByName(databaseName, storeName, names, dotnetref) {
    try {
        console.log(names);
        var searchTerm = names.toUpperCase();
        const indexedDB =
            window.indexedDB ||
            window.mozIndexedDB ||
            window.webkitIndexedDB ||
            window.msIndexedDB ||
            window.shimIndexedDB;
        if (!indexedDB) {
            console.log('IndexedDB could not be found in this browser.');
        }
        var version = localStorage.getItem('version');
        let request = window.indexedDB.open(databaseName, version);
        return new Promise((resolve, reject) => {
            request.onsuccess = function () {
                console.log('Database opened successfully');
                const db = request.result;
                const transaction = db.transaction(storeName, 'readonly');
                const store = transaction.objectStore(storeName);
                console.log(store);
                const index = store.index('ArticleId');
                console.log(index);
                console.log(searchTerm);

                const results = [];
                index.openCursor().onsuccess = function (event) {
                    const cursor = event.target.result;
                    if (cursor) {
                        const articleId = cursor.key[0]; // Assuming ArticleId is the first part of the compound key
                        if (articleId.includes(searchTerm) || searchTerm.length >= 3 && articleId.split('').some((_, i, arr) => arr.slice(i, i + 3).join('') === searchTerm)) {
                            results.push(cursor.value);
                        }
                        cursor.continue();
                    } else {
                        if (results.length > 0) {
                            console.log(results)
                            dotnetref.invokeMethodAsync('ProcessAndStoreResults', JSON.stringify(results))
                            resolve(results);

                        } else {
                            console.log('No matching articles found');
                            reject('No matching articles found');
                        }
                    }
                };
            };
            request.onerror = function (event) {
                console.error("Database error: " + event.target.error);
                reject("Database error: " + event.target.error);
            };
        });
    } catch (error) {
        console.log("error " + error);
        throw error;
    }
}
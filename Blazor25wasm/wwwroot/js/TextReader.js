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
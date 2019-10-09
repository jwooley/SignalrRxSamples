/*jshint esversion: 6 */
let get_guid = function () {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c === 'x' ? r : r & 0x3 | 0x8;
        return v.toString(16);
    });
};

const resultsList = document.getElementById('results');
const box = document.getElementById('input');
const toLanguage$ = Rx.Observable.from(["de", "es", "zh-CHT", "fr", "it", "ar", "ht", "he", "ja", "ko", "no", "ru", "th", "tlh"]);
const translate$ = (sourceDest) => {

    return Rx.Observable.ajax({
        url: 'https://api.cognitive.microsofttranslator.com/translate?api-version=3.0&to=' + sourceDest.to + '&from=' + sourceDest.from,
        method: 'POST',
        crossDomain: true,
        headers: {
            'Content-Type': 'application/json',
            'Ocp-Apim-Subscription-Key': 'e388ab2d29f94bb488f6043eb9be7b47',
            'X-ClientTraceId': get_guid()
        },
        body: JSON.stringify([{ 'Text': sourceDest.input }])
    });
};

const textChange$ = Rx.Observable.fromEvent(box, 'keyup')
    .do(_ => resultsList.innerHTML = "")
    .debounceTime(500)
    .map(_ => box.value)
    .filter(input => input.length > 0)
    .flatMap(input => toLanguage$.map(lang => {
        return { from: 'en-us', to: lang, input: input };
    }))
    .flatMap(toFrom => translate$(toFrom)
        .map(result => {
            return {
                lang: toFrom.to,
                translated: result.response[0].translations[0].text
            };
        })
        // .flatMap(result => translate$({from: result.lang, input: result.translated, to: 'en-us'})
        //   .map(result2 => {
        //     return {
        //       lang: result.lang, 
        //       translated:result.response[0].translations[0].text}
        //     }))
        .takeUntil(textChange$)
    );

textChange$.subscribe(
    translation => {
        var txtnode = document.createTextNode(translation.lang + ': ' + translation.translated);
        var listItem = document.createElement('li');
        listItem.appendChild(txtnode);
        resultsList.appendChild(listItem);
    }, error => {
        console.log('err: ' + error);
    });
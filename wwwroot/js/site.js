// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function deletesong(element)
{
    let request = new XMLHttpRequest();
    function reqReadyStateChanche() {
        if (request.readyState == 4 && request.status == 200) {
            let response = JSON.parse(request.responseText);
            let div = document.getElementById(response.id);
            if (response.deleted)
            {
                div.lastElementChild.firstElementChild.innerText = 'Восстановить';
            }
            else
            {
                div.lastElementChild.firstElementChild.innerText = 'Удалить';
            }
        }
    }
    let body = "id=" + element.value;
    request.open("POST", "/Tracks/DeleteSong");
    request.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    request.onreadystatechange = reqReadyStateChanche;
    request.send(body);
}
function addsong(element)
{
    let request = new XMLHttpRequest();
    function reqReadyStateChanche() {
        if (request.readyState == 4 && request.status == 200) {
            let response = JSON.parse(request.responseText);
            let div = document.getElementById(response.id);
            if (response.added) {
                div.lastElementChild.firstElementChild.innerText = 'Удалить';
            }
            else {
                div.lastElementChild.firstElementChild.innerText = 'Добавить';
            }
        }
    }
    let body = "id=" + element.value;
    request.open("POST", "/Tracks/AddSong");
    request.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    request.onreadystatechange = reqReadyStateChanche;
    request.send(body);
}
function getAudioData(id)
{
    let request = new XMLHttpRequest();
    function reqReadyStateChanche() {
        if (request.readyState == 4 && request.status == 200)
        {
            let response = request.responseText;
            let audio = document.getElementById('VsyaAudioVsehAudio');
            audio.pause();
            audio.setAttribute('src', 'data:audio/mp3;base64,' + response);
            audio.play();
        }
    }
    let body = "id=" + id;
    request.open("POST", "/Tracks/GetSongData");
    request.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    request.onreadystatechange = reqReadyStateChanche;
    request.send(body);
}
var c = true;

var x = '';

function test() {
    if (c) {
        document.getElementById("text").innerText = "Hello, JS function is Working";
        c = false;
    } else {
        document.getElementById("text").innerText = "TEXT";
        c = true;
    }
}

async function getRequest() {

    let url = 'http://localhost:8080/api/cipher';

    let res = await fetch(url, {
        method: 'GET',
    });

    if (res.ok) {
        let ret = await res.text();
        document.getElementById("result").innerText = ret;

    } else {
        return `HTTP error: ${res.status}`;
    }
}

async function getmockCipher() {
    let url = 'http://localhost:8080/api/mockcipher';

    let res = await fetch(url, {
        method: 'GET',
    });

    if (res.ok) {
        let ret = await res.text();
        return ret;

    } else {
        return `HTTP error: ${res.status}`;
    }
}

async function postRequest() {
    let url = 'http://localhost:8080/api/cipher';

    console.log("X ===> " + x);
    let res = await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'text/html',
        },
        body: x,
    });

    if (res.ok) {
        let ret = await res.text();

        console.log(res);
        console.log(ret);
        document.getElementById("result").innerText = ret;

    } else {
        return `HTTP error: ${res.status}`;
    }
}

getmockCipher().then(ret => { x = ret; console.log(x);})
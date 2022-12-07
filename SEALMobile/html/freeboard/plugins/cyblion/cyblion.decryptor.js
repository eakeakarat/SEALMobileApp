function cyblion_decrypt(ciphertext) {
    
    let url = 'http://localhost:8080/api/cipher';

    let res = await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'text/html',
        },
        body: ciphertext,
    });

    if (res.ok) {
        let plaintext = await res.text();

        console.log(res);
        console.log(plaintext);

        return plaintext;

    } else {
        return `HTTP error: ${res.status}`;
    }

}

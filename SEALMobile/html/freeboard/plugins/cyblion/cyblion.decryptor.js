function cyblion_decrypt(ciphertext) {
	let x = parseFloat(ciphertext) + 10.2;
	// let plaintext = ciphertext;
	let plaintext = x;

	console.log(typeof(ciphertext))
	console.log("plaintext:  " + plaintext);
	console.log("cipher:  " + ciphertext);

	//window.location = 'myapp://custom?args=COMPLETE';

	return plaintext;
}

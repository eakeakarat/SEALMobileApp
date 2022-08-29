const express = require('express')
const bodyParser = require("body-parser");
const router = express.Router();
const app = express()
const port = 9000

app.use(bodyParser.json({limit: '10mb', extended: true}))
app.use(bodyParser.urlencoded({limit: '10mb', extended: true}))
app.use(express.json({ limit: '50mb', extended: true }));
app.use(express.urlencoded({ limit: '50mb', extended: true }));
app.use("/", router);

app.listen(port, () => {
    console.log(`listening on port ${port}`)
})

app.get('/', (req, res) => {
    res.send('Hello World!')
})

router.post('/parms', (request, response) => {
    // console.log(request.body)
    console.log("post parms")
    const fs = require('fs');
    const content = request.body.parms;
    fs.writeFile('key/parms.txt', content, err => {
        if (err) {
            console.error(err);
        }
        // file written successfully
    });
    response.send("post")
})

router.get('/parms', (request, response) => {
    // console.log(request.body)
    console.log("get parms")
    const fs = require('fs');
    fs.readFile('key/parms.txt', 'utf8', (err, data) => {
        if (err) {
            console.error(err);
            return;
        }
        // console.log(data);
        response.send(data)
    });
})

router.post('/pk', (request, response) => {
    // console.log(request.body)
    console.log("post publicKey")
    const fs = require('fs');
    const pkBase64 = request.body.pkBase64;
    fs.writeFile('key/publicKey.txt', pkBase64, err => {
        if (err) {
            console.error(err);
        }
        // file written successfully
    });
    // console.log(pkBase64)
    response.send("post")
})

router.post('/rlk', (request, response) => {
    console.log("post relinKeys")
    // console.log(request.body)
    const fs = require('fs');
    const rlkBase64 = request.body.rlkBase64;
    fs.writeFile('key/relinKey.txt', rlkBase64, err => {
        if (err) {
            console.error(err);
        }
        // file written successfully
    });
    // console.log(rlkBase64)
    response.send("post")
})

router.get('/pk', (request, response) => {
    console.log("get publicKey")
    // console.log(request.body)
    const fs = require('fs');
    fs.readFile('key/publicKey.txt', 'utf8', (err, data) => {
        if (err) {
            console.error(err);
            return;
        }
        // console.log(data);
        response.send(data)
    });
})

router.get('/rlk', (request, response) => {
    console.log("get relinKeys")
    // console.log(request.body)
    const fs = require('fs');
    fs.readFile('key/relinKey.txt', 'utf8', (err, data) => {
        if (err) {
            console.error(err);
            return;
        }
        // console.log(data);
        response.send(data)
    });
})

router.post('/result', (request, response) => {
    console.log("post resultCipher")
    // console.log(request.body)
    const fs = require('fs');
    const result = request.body.result;
    fs.writeFile('key/resultCipher.txt', result, err => {
        if (err) {
            console.error(err);
        }
        // file written successfully
    });
    // console.log(result)
    response.send("post")
})

router.get('/result', (request, response) => {
    console.log("get resultCipher")
    // console.log(request.body)
    const fs = require('fs');
    fs.readFile('key/resultCipher.txt', 'utf8', (err, data) => {
        if (err) {
            console.error(err);
            return;
        }
        // console.log(data);
        response.send(data)
    });
})


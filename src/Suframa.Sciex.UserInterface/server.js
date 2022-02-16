const express = require('express');

const port = 3000 || process.env.PORT;
const app = express();

app.engine('html', require('ejs').renderFile);

app.set('view engine', 'html');
app.set('views', 'www');

app.use('/', express.static('www', { index: false }));

app.get('/*', (req, res) => {
	res.render('./index', { req, res });
});

app.listen(port, () => {
	console.log(`Listening on: http://localhost:${port}`);
});

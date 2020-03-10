const app = require('express')();
const http = require('http').Server(app);
const https = require('https');
const fs = require('fs');

const certRoot = `/etc/letsencrypt/live/research.supine.dev`;
const port = 3018;
const server = https.createServer({
    key: fs.readFileSync(`${certRoot}/privkey.pem`),
    cert: fs.readFileSync(`${certRoot}/cert.pem`)
}, app).listen(port, function () {
    console.log(`[HTTPS] Listening on port ${port}`)
})
const io = require('socket.io')(server);

let count = 0;
io.on('connection', function(socket) {

    var x = socket.$emit;

    socket.$emit = function(){
        var event = arguments[0];
        var feed  = arguments[1];

        //Log
        console.log(event + ":" + feed);

        //To pass listener
        x.apply(this, Array.prototype.slice.call(arguments));
    };


    socket.emit('hi');
    console.log(`New connection [${++count} users]`)


    socket.on('rooms:join', code => {
        code = code.toUpperCase();
        console.log(`Client is joining room ${code}`);
        socket.join(code, function() {
            socket.emit('rooms:joined', code);
            io.sockets.to(code).emit('message', 'A new client has joined your room!')
            let rooms = Object.keys(socket.rooms);
            console.log(rooms);
        });
    });

    socket.on('rooms:create', () => {
        // generate a code
        // join the room
        // send back the code

        let code = generateCode(4);

        socket.join(code, function() {
            console.log(`Creating room ${code}`);
            socket.emit('rooms:joined', code);
        });

    });

    socket.on('player:position', (pos) => {
        let room = (Object.keys(socket.rooms)).filter(r => r.length === 4)[0];
        io.sockets.to(room).emit('player:position', pos);
    })


    socket.on('disconnect', function() {
        console.log(`Disconnected [${--count} users]`)
    });
    socket.on('game:action', function(action) {
        console.log(`Game action ${action}`);
        let room = (Object.keys(socket.rooms)).filter(r => r.length === 4)[0];
        io.sockets.to(room).emit('game:action', action);
    });
});

function generateCode(length) {
    const alphabet = `ABCDEFGHIJKLMNOPQRSTUVWXYZ`;
    let code = "";
    for (let i = 0; i < length; i++) {
        code += alphabet[Math.floor(alphabet.length * Math.random())]
    }
    return code;
}
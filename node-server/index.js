const express = require('express');
const app = express();
const http = require('http').Server(app);
const https = require('https');
const fs = require('fs');
const tls = require('tls');

const certRoot = `/etc/letsencrypt/live/research.supine.dev`;
const port = 3018;
const server = https.createServer({
    key: fs.readFileSync(`${certRoot}/privkey.pem`),
    cert: fs.readFileSync(`${certRoot}/cert.pem`)
}, app).listen(port, function () {
    console.log(`[HTTPS] Listening on port ${port}`)
});
const io = require('socket.io')(server);

app.use(express.static('static'));


let count = 0;
let currentGame = null;

let activeGames = [];

io.on('connection', function(socket) {

    var x = socket.$emit;

    function getRoomCode() {
        let room = (Object.keys(socket.rooms)).filter(r => r.length === 4)[0];
        socket._gameRoom = room;
        return room;
    }
    function getRoomSocket() {
        return io.sockets.to(getRoomCode())
    }
    socket.$emit = function(){
        var event = arguments[0];
        var feed  = arguments[1];

        //Log
        console.log(event + ":" + feed);

        //To pass listener
        x.apply(this, Array.prototype.slice.call(arguments));
    };

    socket._isGame = socket.request.headers['user-agent'].includes('websocket-sharp');


    //socket.emit('hi');
    console.log(`New connection [${++count} users] from ${socket._isGame ? "game" : "web"}`);
    socket.on('disconnect', function() {
        if (socket._isGame)  {
            getRoomSocket().emit(`server:game-disconnected`);
            console.log(`Game for ${socket._gameRoom} disconnected`);

            activeGames.splice(activeGames.indexOf(getRoomCode()), 1);
        }
        if (currentGame === socket._gameRoom) currentGame = null;
        console.log(`Disconnected [${--count} users]`)
    });

    socket.on('declare:master', function() {
        socket.join('master', function() {
            if (currentGame) socket.emit('rooms:new', currentGame);
        });
    })
    
    
    socket.on('rooms:join', code => {
        code = code.toUpperCase();


        if (activeGames.indexOf(code) === -1) return socket.emit('server:unknown-code');

        code = code.toUpperCase();
        console.log(`Client is joining room ${code}`);
        socket.join(code, function() {
            socket.emit('rooms:joined', code);
            io.sockets.to(code).emit('message', 'A new client has joined your room!')
            let rooms = Object.keys(socket.rooms);
            console.log(rooms);
            
            if (!socket._isGame) {
                // New web connection, signal for backfill
                io.sockets.to(code).emit('rooms:backfill');
            }
            
        });
    });

    socket.on('rooms:disconnect', function() {
        try {
            (Object.keys(socket.rooms)).filter(r => r.length === 4).map(socket.leave);
        } catch (e) {}
        socket.emit('rooms:disconnected');
    })

    socket.on('rooms:create', () => {
        // generate a code
        // join the room
        // send back the code

        let code = generateCode(4);
        currentGame = code;

        activeGames.push(code);

        socket.join(code, function() {
            console.log(`Creating room ${code}`);
            socket.emit('rooms:joined', code);
        });

        io.sockets.to('master').emit('rooms:new', code);

    });

    
    
    socket.on('player:position', (pos) => {
        getRoomSocket().emit('player:position', pos);
    });


    
    socket.on('game:action', function(action, ...args) {
        //console.log(`Game action ${action}`, args);
        getRoomSocket().emit('game:action', action, ...args);
    });
    
    // Blobs come in as arrays from Unity. I should handle it better in C#, but it's much much easier to handle here serverside.
    // Better to do it here rather than on the client side anyway.
    socket.on('object:position', function(blob) { getRoomSocket().emit('object:position', blob[0]); });
    socket.on('entity:create', function(blob) {  /*console.log("entity:create", blob); */ getRoomSocket().emit('entity:create', blob[0]); });

    socket.on('entity:destroy', function(blob) { /*console.log("entity:destroy", blob); */ getRoomSocket().emit('entity:destroy', blob[0]); });
    socket.on('room:update', function(blob) { /*console.log("room:update", blob); */ getRoomSocket().emit('room:update', blob[0]); });
});

function generateCode(length) {
    const alphabet = `ABCDEFGHIJKLMNOPQRSTUVWXYZ`;
    let code = "";
    for (let i = 0; i < length; i++) {
        code += alphabet[Math.floor(alphabet.length * Math.random())]
    }
    return code;
}

/* SUPINE */
/* app.js (it's all in here) */
const socket = io("wss://play.supine.dev");

const IS_MASTER = document.location.hash.substr(1) === "master";


socket.on('connection', function() {
    console.log('Connectioned to research server!');
});
socket.on('connected', function() {
    console.log('Connected to research server!');
});

function reset() {
    document.querySelector('#map').innerHTML = `<div id="room-bg"></div>`;
    document.querySelector('#code-name').innerHTML = "";
}

socket.on('connect', function() {
    if (IS_MASTER) {
        let newCode;
        socket.emit('declare:master');
        socket.on('rooms:new', function(code) {
            newCode = code;
            reset();
            socket.emit('rooms:disconnect');
        });
        socket.on('rooms:disconnected', function() {
            socket.emit('rooms:join', newCode);
        })
    }
});
socket.on('game:action', function(action) {
    console.log('Game action from client', action);
});
socket.on('rooms:joined', function(code) {
    console.log(`Successfully joined room ${code}`);
    document.querySelector('#code-name').innerHTML = code;
    document.querySelector('#play').classList.remove('hide');
    document.querySelector('#login').classList.add('hide');

    document.querySelector('#items').classList.remove('hide');
});
socket.on('message', console.log);

socket.on('player:position', function(/* array [x,z] */ pos) {
    console.warn("player:position is deprecated.");
    //console.log(`Changing player position to`, pos);

    [x,z] = checkPos(pos);

    document.querySelector('#player').style.left = `${x * 100}%`
    document.querySelector('#player').style.bottom = `${z * 100}%`

});

const els = {
    "player": document.querySelector('#player')
};


let objectTranslationOffset = `translate(-50%,50%)`;

socket.on('object:position', function(blob) {
    //console.log('object:position', blob);
    let el = els[blob.identifier];
    if (!el) return;

    [x,z] = checkPos(blob.position);

    /*document.querySelector(`#${blob.identifier}`)
    document.querySelector(`#${blob.identifier}`)*/

    el.style.left = `${x * 100}%`;
    el.style.bottom = `${z * 100}%`;

    if (blob.rotation) {
        el.style.transform = `rotate(${ blob.rotation - 360 }deg) ${objectTranslationOffset}`;
    }

});

socket.on('server:game-disconnected', function() {
    document.body.classList.add('disconnected');
    document.querySelector('#server h1').innerText = `Disconnected`;
    document.querySelector('#server').classList.remove('hide');
});

socket.on('server:unknown-code', function() {
    document.body.classList.add('disconnected');
    document.querySelector('#server h1').innerText = `Unknown code`;
    document.querySelector('#server').classList.remove('hide');
});



function rotate(cx, cy, x, y, angle) {
    console.log(`Rotating point ${[cx,cy]} ${angle}deg around ${[x,y]}`)
    var radians = (Math.PI / 180) * angle,
        cos = Math.cos(radians),
        sin = Math.sin(radians),
        nx = (cos * (x - cx)) + (sin * (y - cy)) + cx,
        ny = (cos * (y - cy)) - (sin * (x - cx)) + cy;
    return [nx, ny];
}


const gridSize = 20;


function delegateMapSpawn(x,y) {
    let item = items.find(i => i.selected);
    if (item) {
        socket.emit('game:action', item.slug, x / gridSize, y / gridSize);
        item.use();
    }
}


(function() {
    let el = document.querySelector('#map');

    el.insertAdjacentHTML('beforeend' ,`<div class="map-clickable"></div>`);
    let _mapClicker = el.lastChild;


    document.head.insertAdjacentHTML('beforeend', `<style>.map-clickable {
grid-template-columns: repeat(${gridSize}, 1fr);
    grid-template-rows: repeat(${gridSize}, 1fr);
}</style>`);

    for (let a = 0; a < gridSize; a++) {
        for (let b = 0; b < gridSize; b++) {
            _mapClicker.insertAdjacentHTML('beforeend', `<div class="map-square"></div>`);
            _mapClicker.lastChild.addEventListener('click', function() {
                delegateMapSpawn(a, b);
            })

        }
    }


    el.addEventListener('click', function(evt) {
        return;

        /*
                el.insertAdjacentHTML('beforeend', `<div class="click-target"></div>`);
                let __c = el.lastChild;

                __c.style.top = `${evt.clientX}px`;
                __c.style.left = `${evt.clientY}px`;
        */
        let rect = evt.srcElement.getBoundingClientRect();
        console.log("x", evt.clientX);
        console.log("y", evt.clientY);
        console.log("x", evt.clientX - rect.left);
        console.log("y", evt.clientY - rect.top);


        let rotation = rotate(evt.offsetX, evt.offsetY, rect.width / 2, rect.height / 2, 135);
        console.log("r0", rotation[0], rect.width);
        console.log("r1", rotation[1], rect.height);

        /*
                el.insertAdjacentHTML('beforeend', `<div class="click-target ct2"></div>`);
                let __c2 = el.lastChild;

                __c2.style.top = `${rotation[0]}px`;
                __c2.style.left = `${rotation[1]}px`;*/

        let [x,z] = [
            evt.offsetX / rect.height,
            (rect.width - evt.offsetY) / rect.width];

        //console.log(x, z);


    });
})();


let remoteMap = [-17, 17];
let remoteCenter = [0, 0];
function updatePositioning(roomPosition) {
    remoteCenter = roomPosition;
}
function checkPos(pos) {
    //console.log(checkPos);
    pos = pos.map((p,i) => p += ((0 - remoteMap[0]) - remoteCenter[i]));
    pos = pos.map(p => p / ((0 - remoteMap[0]) + remoteMap[1]));

    return pos;
}

document.querySelector('#code').addEventListener('input', function() {
    let _el = document.querySelector('#code');
    if (_el.value.toString().length === 4) {
        document.querySelector('#submit-room').removeAttribute('disabled');
    } else {
        document.querySelector('#submit-room').setAttribute('disabled', 'true');
    }
});
document.querySelector('#code').addEventListener('keydown', function(e) {
    if (e.keyCode === 13) {
        submitRoom();
    }
});
function submitRoom() {
    let code = document.querySelector('#code').value;


    console.log(`Joining room ${code}`);
    socket.emit('rooms:join', code);
}
document.querySelector('#submit-room').addEventListener('click', submitRoom)

Array.from(document.querySelectorAll('.action-button')).forEach(el => {
    el.addEventListener('click', function() {
        socket.emit('game:action', el.dataset.action);
    })
});

function createEntity(identifier, className) {
    document.querySelector('#map').insertAdjacentHTML('beforeend', `<div class="blob blob--${className}" id="${identifier}"><div class="blob-content"></div></div>`);
    return document.querySelector('#map').lastChild;
}

socket.on('entity:create', blob => {
    console.log("create", blob);
    if (!els[blob.identifier]) {
        els[blob.identifier] = createEntity(blob.identifier, blob.className);
    }
})
socket.on('position:create', blob => {
    console.log("create", blob);
    if (!els[blob.identifier]) {
        els[blob.identifier] = createEntity(blob.identifier, blob.className);
    }
})
socket.on('entity:destroy', blob => {
    console.log("destroy", blob);
    els[blob.identifier].classList.add('kill');
    setTimeout(function() {
        els[blob.identifier].remove();
    }, 2000);
})
socket.on('room:update', blob => console.log("room", blob));

function getColorStr(color) {
    return `rgb(${color.map(c => `${c * 100}%`).join(',')})`;
}

let ROOMS_SPIKED = {};
let CURRENT_ROOM;
let SPIKE_TRAP_ITEM;

socket.on('room:update', room => {
    if (!room) return console.warn("Empty room data");
    if (!ROOMS_SPIKED[room.identifier]) ROOMS_SPIKED[room.identifier] = false;
    CURRENT_ROOM = room.identifier;

    if (!ROOMS_SPIKED[room.identifier]) {
        SPIKE_TRAP_ITEM.setEnabled(true)
    } else {
        SPIKE_TRAP_ITEM.setEnabled(false);
    }



    let rgb = getColorStr(room.color);
    document.querySelector('#map').style.backgroundColor = rgb;
    document.querySelector('#map').classList.add('no-anim');
    setTimeout(function() {
        document.querySelector('#map').classList.remove('no-anim');
    }, 50);
    updatePositioning(room.position);

    const doorLetters = ["N", "E", "W", "S"];

    document.querySelector('#room-bg').innerHTML = "";
    doorLetters.forEach((letter, i) => {

        let door = room.doors.find(r => r.direction === letter);
        if (!door) { door = {direction: letter, exists: false, locked:null,open:null} }
        // need to add locking
        document.querySelector('#room-bg').insertAdjacentHTML('beforeend', `<div class="wall" data-door="${door.direction}" data-exists="${door.exists}" data-open="${door.open}" data-locked="${door.locked}"></div>`)
    });

    document.querySelector('#room-bg').style.backgroundImage = `url("/media/sprites/rooms/${room.prefabName}.png")`;
})

class Cursor {
    static set(item) {
        document.querySelector('#cursor').innerHTML = `<div class="item-cursor" style="background-color:${item.color}80;"><div class="item-cursor-image" style="background-image: url('${item.getImageURL()}')"></div></div>`;
    }
    static empty() {
        document.querySelector('#cursor').innerHTML = ``;
    }
}

class Item {
    constructor(data) {
        this.name = data.name;
        this.slug = data.slug;
        this.cooldown = data.cooldown * 1000;
        this.color = data.color;

        this.oneUsePerRoom = data.oneUsePerRoom || false;

        this.enabled = true;
        this.selected = false;
    }

    getImageURL() {
        return `/media/sprites/items/${this.slug}.png`
    }

    menuItemElement() {
        let menuItem = document.createElement('div');
        menuItem.className = `menu-item menu-item--${this.slug}`;
        menuItem.innerHTML = `<div class="menu-bg"></div><div class="item-image" style="background-image: url('${this.getImageURL()}')"></div><div class="item-cooldown-text hide"></div><div class="menu-text">${this.name}</div>`;

        let __item = this;
        menuItem.addEventListener('click', function() {
            if (!__item.enabled) return;
            __item.toggleActive();

            if (__item.selected) {
                __item.playAudio("menu");
            } else {
                __item.playAudio("no");
            }
        });
        this.menuItem = menuItem;
        return menuItem;
    }


    toggleActive() {
        this.setActive(!this.selected);
    }

    setActive(status) {
        if (status === true) {
            items.forEach(item => item.setActive(false));
            this.menuItem.classList.add('selected');
            this.menuItem.querySelector('.menu-bg').style.backgroundColor = this.color;
            if (!this.oneUsePerRoom) {
                Cursor.set(this);
            } else {
                this.use();
            }

        } else {
            this.menuItem.classList.remove('selected');
            this.menuItem.querySelector('.menu-bg').style.backgroundColor = `initial`;
            Cursor.empty();
        }
        this.selected = status;
    }

    setEnabled(status) {
        this.enabled = status;
        if (status) {
            if (this.oneUsePerRoom) {
                this.menuItem.querySelector('.item-cooldown-text').classList.add('hide');
                this.menuItem.querySelector('.item-cooldown-text').innerText = "";
            }
            this.menuItem.classList.remove('disabled');
        } else {
            if (this.oneUsePerRoom) {
                this.menuItem.querySelector('.item-cooldown-text').classList.remove('hide');
                this.menuItem.querySelector('.item-cooldown-text').innerText = "";
            }
            this.menuItem.classList.add('disabled');
        }
    }

    updateCooldown(remaining) {
        if (Math.floor(remaining / 1000) === 0) {
            // finished
            this.menuItem.querySelector('.item-cooldown-text').classList.add('hide');
            this.menuItem.querySelector('.item-cooldown-text').innerText = "";
        } else {
            this.menuItem.querySelector('.item-cooldown-text').classList.remove('hide');
            this.menuItem.querySelector('.item-cooldown-text').innerText = Math.floor(remaining / 1000);
        }
    }

    cooldownExpiredAnimation() {
        let __item = this;

        __item.menuItem.querySelector('.menu-bg').style.backgroundColor = this.color;
        __item.menuItem.classList.add('cooldown-finished');

        setTimeout(function() {
            __item.menuItem.querySelector('.menu-bg').style.backgroundColor = `initial`;
            __item.menuItem.classList.remove('cooldown-finished');
        }, 300);
    }

    startCooldown(length) {
        let __item = this;

        let remaining = length;
        __item.updateCooldown(length);

        let tickInterval = setInterval(function() {
            remaining -= 1000;
            __item.updateCooldown(remaining);
        }, 1000);

        setTimeout(function() {
            clearInterval(tickInterval);
            __item.setEnabled(true);
            __item.cooldownExpiredAnimation();
        }, length);
    }

    playAudio(override) {
        let audio = new Audio(`/media/audio/${override ? override : this.slug}.wav`);
        audio.volume = 1.0;
        audio.play();
    }

    use() {
        // ROOMS_SPIKED
        if (this.oneUsePerRoom) {
            if (ROOMS_SPIKED[CURRENT_ROOM]) return;
            // Spike this room;
            socket.emit('game:action', this.slug, 0.2, 0.2);
            this.setActive(false);
            this.setEnabled(false);
            this.playAudio();
            ROOMS_SPIKED[CURRENT_ROOM] = true;
            this.selected = false;
            return;
        }

        this.setActive(false);
        this.setEnabled(false);
        this.startCooldown(this.cooldown);
        this.playAudio();

        let __item = this;




        setTimeout(function() {
            __item.setEnabled(true);
        }, this.cooldown);
    }
}

const items = [
    new Item({
        name: "Bomb",
        slug: "bomb",
        cooldown: 4,
        color: "#888888"}),
    new Item({
        name: "Health Potion",
        slug: "healthPotion",
        cooldown: 8,
        color: "#b73c96"}),
    new Item({
        name: "Spike Trap",
        slug: "spikeTrap",
        cooldown: 15,
        oneUsePerRoom: true,
        color: "#674319"})
];

SPIKE_TRAP_ITEM = items[2];

const _items = document.querySelector('#items');
_items.innerHTML = ``;
items.forEach(item => {
    _items.insertAdjacentElement('beforeend', item.menuItemElement())
});

const cursor = document.querySelector('#cursor');
document.addEventListener('mousemove', function(evt) {
    cursor.style.left = `${evt.pageX}px`;
    cursor.style.top = `${evt.pageY}px`;
});
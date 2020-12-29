const SCALE = 5;
let cols = 5;
let rows = 1;
let width = 16;
let height = 18;
let scaledWidth = SCALE * width;
let scaledHeight = SCALE * height;
const CYCLE_LOOP = [0, 1, 0, 2];
const FACING_DOWN = 0;
const FACING_UP = 1;
const FACING_LEFT = 2;
const FACING_RIGHT = 3;
const FRAME_LIMIT = 12;
const MOVEMENT_SPEED = 1;
const ATTACK_LOOP = [0, 1, 2, 3, 4];
let canvas = document.getElementById('canvasAnim');
let ctx = canvas.getContext('2d');
let keyPresses = {};
let currentDirection = FACING_DOWN;
let currentLoopIndex = 0;
let frameCount = 0;
let positionX = 0;
let positionY = 0;
let img = document.getElementById('imgelem');
let walkImg = document.getElementById('imgelem');
let wizImg = document.getElementById('wizImg');
let multiplier = 1;

//----------------------- Attack functions ------------------------------------------
export function initAttack(sprite) {
    if (sprite === 'mage') {
        img = wizImg;
    }
    height = img.height / rows;
    width = img.width / cols;
    window.addEventListener('keydown', keyDownListener, false);
    function keyDownListener(event) {
        keyPresses[event.key] = true;
    }

    window.addEventListener('keyup', keyUpListener, false);
    function keyUpListener(event) {
        keyPresses[event.key] = false;
    }
    window.requestAnimationFrame(attackLoop);
}
function attackLoop() {
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    let hasMoved = false;
    if (keyPresses.a) {
        hasMoved = true;
    }
    if (hasMoved) {
        frameCount++;
        if (frameCount >= FRAME_LIMIT) {
            frameCount = 0;
            currentLoopIndex++;
            if (currentLoopIndex >= ATTACK_LOOP.length) {
                currentLoopIndex = 0;
            }
        }
    }
    if (!hasMoved) {
        currentLoopIndex = 0;
    }
    drawFrame(ATTACK_LOOP[currentLoopIndex], 0, 0, 0);
    window.requestAnimationFrame(attackLoop);
}
// -------------------------- Shared Functions -------------------------------

function drawFrame(frameX, frameY, canvasX, canvasY) {
    ctx.drawImage(img,
        frameX * width * multiplier, frameY * height, width * multiplier, height,
        canvasX, canvasY, scaledWidth * multiplier, scaledHeight);
}

// -------------------------- Dude walking around with asdw keys --------------------
export function initCanvasAnimate() {
    img = walkImg;
    window.addEventListener('keydown', keyDownListener, false);
    function keyDownListener(event) {
        keyPresses[event.key] = true;
    }

    window.addEventListener('keyup', keyUpListener, false);
    function keyUpListener(event) {
        keyPresses[event.key] = false;
    }
    window.requestAnimationFrame(walkAroundLoop);
}


function walkAroundLoop() {
    ctx.clearRect(0, 0, canvas.width, canvas.height);

    let hasMoved = false;

    if (keyPresses.w) {
        moveCharacter(0, -MOVEMENT_SPEED, FACING_UP);
        hasMoved = true;
    } else if (keyPresses.s) {
        moveCharacter(0, MOVEMENT_SPEED, FACING_DOWN);
        hasMoved = true;
    }

    if (keyPresses.a) {
        moveCharacter(-MOVEMENT_SPEED, 0, FACING_LEFT);
        hasMoved = true;
    } else if (keyPresses.d) {
        moveCharacter(MOVEMENT_SPEED, 0, FACING_RIGHT);
        hasMoved = true;
    }

    if (hasMoved) {
        frameCount++;
        if (frameCount >= FRAME_LIMIT) {
            frameCount = 0;
            currentLoopIndex++;
            if (currentLoopIndex >= CYCLE_LOOP.length) {
                currentLoopIndex = 0;
            }
        }
    }
    if (!hasMoved) {
        currentLoopIndex = 0;
    }
    drawFrame(CYCLE_LOOP[currentLoopIndex], currentDirection, positionX, positionY);
    window.requestAnimationFrame(walkAroundLoop);
}
function moveCharacter(deltaX, deltaY, direction) {
    if (positionX + deltaX > 0 && positionX + scaledWidth + deltaX < canvas.width) {
        positionX += deltaX;
    }
    if (positionY + deltaY > 0 && positionY + scaledHeight + deltaY < canvas.height) {
        positionY += deltaY;
    }
    currentDirection = direction;
}

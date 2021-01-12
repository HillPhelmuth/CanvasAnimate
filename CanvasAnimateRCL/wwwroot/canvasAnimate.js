const SCALE = 4;
let cols = 1;
let rows = 10;
let width = 16;
let height = 18;
let scaledWidth = SCALE * width;
let scaledHeight = SCALE * height;
const CYCLE_LOOP = [0, 1, 0, 2];
const FACING_DOWN = 0;
const FACING_UP = 1;
const FACING_LEFT = 2;
const FACING_RIGHT = 3;
const FRAME_LIMIT = 6;
const MOVEMENT_SPEED = 1;
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

// -------------------------- Shared Functions -------------------------------

function drawFrame(frameX, frameY, canvasX, canvasY) {
    ctx.drawImage(img,
        frameX * width, frameY * height, width, height,
        canvasX, canvasY, scaledWidth, scaledHeight);
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

export function ping() {
    console.log("canvasAnimate.js module imported");
}
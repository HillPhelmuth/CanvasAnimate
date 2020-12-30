const SCALE = 3;
const FRAME_LIMIT = 9;
let width = 59;
let height = 71;
let scaledWidth = width * SCALE;
let scaledHeight = height * SCALE;
let frameCount = 0;
let currentLoopIndex = 0;
let positionX = 0;
let positionY = 0;

let canvas = document.getElementById('canvasAnim');
let ctx = canvas.getContext('2d');
let spriteFrames = [];
let keyPresses = {};
let img = document.getElementById("attack");
let image = new Image();
let attackData = null;
let jumpAttackData = null;
let runData = null;
export function attack(spriteData) {
    var frameLog = JSON.stringify(spriteData.frames);
    console.log("Frame log: " + frameLog);
    spriteFrames = spriteData.frames;
    //let image = new Image();
    image.src = spriteData.imgUrl;
    img.src = spriteData.imgUrl;
    window.requestAnimationFrame(animateLoop);
}
function animateLoop() {
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    frameCount++;
    var moving = true;
    if (frameCount >= FRAME_LIMIT) {
        frameCount = 0;
        currentLoopIndex++;
        if (currentLoopIndex >= spriteFrames.length) {
            return;
        } else {
            console.log("Specs: " + JSON.stringify(spriteFrames[currentLoopIndex].specs));
        }
    }
    
    var frameX = spriteFrames[currentLoopIndex].specs.x;
    var frameY = spriteFrames[currentLoopIndex].specs.y;
    width = spriteFrames[currentLoopIndex].specs.w;
    height = spriteFrames[currentLoopIndex].specs.h;
    scaledWidth = width * SCALE;
    scaledHeight = height * SCALE;
    drawFrame(frameX, frameY, positionX, positionY);
    window.requestAnimationFrame(animateLoop);


}
function drawFrame(frameX, frameY, canvasX, canvasY) {
    ctx.drawImage(image,
        frameX, frameY, width, height,
        canvasX, canvasY, scaledWidth, scaledHeight);
}
const MOVEMENT_SPEED = 2;
export function initRun(spriteData, attack, jumpAttack) {
    var frameLog = JSON.stringify(spriteData.frames);
    console.log("Frame log: " + frameLog);
    spriteFrames = spriteData.frames;
    runData = spriteData;
    attackData = attack;
    jumpAttackData = jumpAttack;
    //let image = new Image();
    image.src = spriteData.imgUrl;
    img.src = spriteData.imgUrl;
    window.addEventListener('keydown', keyDownListener, false);
    function keyDownListener(event) {
        keyPresses[event.key] = true;
    }

    window.addEventListener('keyup', keyUpListener, false);
    function keyUpListener(event) {
        keyPresses[event.key] = false;
    }

    window.requestAnimationFrame(runLoop);
}

function runLoop() {
    ctx.clearRect(0, 0, canvas.width, canvas.height);

    let hasMoved = false;

    if (keyPresses.w) {
        image.src = runData.imgUrl;
        img.src = runData.imgUrl;
        moveCharacter(0, -MOVEMENT_SPEED);
        hasMoved = true;
    } else if (keyPresses.s) {
        image.src = runData.imgUrl;
        img.src = runData.imgUrl;
        moveCharacter(0, MOVEMENT_SPEED);
        hasMoved = true;
    }

    if (keyPresses.a) {
        image.src = runData.imgUrl;
        img.src = runData.imgUrl;
        moveCharacter(-MOVEMENT_SPEED, 0);
        hasMoved = true;
    } else if (keyPresses.d) {
        image.src = runData.imgUrl;
        img.src = runData.imgUrl;
        moveCharacter(MOVEMENT_SPEED, 0);
        hasMoved = true;
    } else if (keyPresses.k) {
        image.src = attackData.imgUrl;
        img.src = attackData.imgUrl;
        hasMoved = true;
    } else if (keyPresses.j) {
        image.src = jumpAttackData.imgUrl;
        img.src = jumpAttackData.imgUrl;
        hasMoved = true;
    }

    if (hasMoved) {
        frameCount++;
        if (frameCount >= FRAME_LIMIT) {
            frameCount = 0;
            currentLoopIndex++;
            if (currentLoopIndex >= spriteFrames.length) {
                currentLoopIndex = 0;
            }
            console.log("Specs: " + JSON.stringify(spriteFrames[currentLoopIndex].specs));
        }
    }
    if (!hasMoved) {
        currentLoopIndex = 0;
    }
    
    var frameX = spriteFrames[currentLoopIndex].specs.x;
    var frameY = spriteFrames[currentLoopIndex].specs.y;
    width = spriteFrames[currentLoopIndex].specs.w;
    height = spriteFrames[currentLoopIndex].specs.h;
    scaledWidth = width * SCALE;
    scaledHeight = height * SCALE;
    drawFrame(frameX, frameY, positionX, positionY);
    window.requestAnimationFrame(runLoop);
}


function moveCharacter(deltaX, deltaY) {
    if (positionX + deltaX > 0 && positionX + scaledWidth + deltaX < canvas.width) {
        positionX += deltaX;
    }
    if (positionY + deltaY > 0 && positionY + scaledHeight + deltaY < canvas.height) {
        positionY += deltaY;
    }
}
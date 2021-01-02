const SCALE = 2;
const FRAME_LIMIT = 15;
let width = 59;
let height = 71;
let scaledWidth = width * SCALE;
let scaledHeight = height * SCALE;
let frameCount = 0;
let currentLoopIndex = 0;
let positionX = 0;
let positionY = 0;
const FACE_RIGHT = "right";
const FACE_LEFT = "left";
const MOVEMENT_SPEED = 2;
let faceCurrent = FACE_RIGHT;
let activeKey = "";
let canvas = document.getElementById('canvasAnim');
let ctx = canvas.getContext('2d');
let spriteFrames = [];
let keyPresses = {};
let img = document.getElementById("imageDisplay");
let image = new Image();
let bombImg = new Image();
let spritesDictionary = {};
let requestId = null;
let isDead = false;

export function initAnimation(spritesheets) {
    spritesDictionary = spritesheets;
   
    bombImg.src = spritesDictionary["Bomb"].imgUrl;
    DotNet.invokeMethodAsync('CanvasAnimateRCL', 'MessageFromJs', 'Initialized actionLoop animation loop');
    image.src = spritesDictionary["Idle"].imgUrl;
    img.src = spritesDictionary["Idle"].imgUrl;
    spriteFrames = spritesDictionary["Idle"].rightFrames;
    window.addEventListener('keydown', keyDownListener, false);
    function keyDownListener(event) {
        keyPresses[event.key] = true;
    }

    window.addEventListener('keyup', keyUpListener, false);
    function keyUpListener(event) {
        keyPresses[event.key] = false;
    }

    requestId = window.requestAnimationFrame(actionLoop);
}
function actionLoop() {
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    drawBomb();
    
    let hasMoved = false;

    if (keyPresses.w) {
        activeKey = "Run";
        moveCharacter(0, -MOVEMENT_SPEED);
        hasMoved = true;
    } else if (keyPresses.s) {
        activeKey = "Run";
        moveCharacter(0, MOVEMENT_SPEED);
        hasMoved = true;
    }
    if (keyPresses.a) {
        faceCurrent = FACE_LEFT;
        activeKey = "Run";
        moveCharacter(-MOVEMENT_SPEED, 0);
        hasMoved = true;
    } else if (keyPresses.d) {
        faceCurrent = FACE_RIGHT;
        activeKey = "Run";
        moveCharacter(MOVEMENT_SPEED, 0);
        hasMoved = true;
    } else if (keyPresses.k) {
        activeKey = "Attack";
        hasMoved = true;
    } else if (keyPresses.j) {
        activeKey = "JumpAttack";
        hasMoved = true;
    } else if (keyPresses.x) {
        activeKey = "Dead";
        hasMoved = true;
    } else if (keyPresses.b) {
        activeKey = "Bomb";
        hasMoved = true;
    } else {
        activeKey = "Idle";
        hasMoved = true;
    }
    if (isDead) {
        return;
    }
    if (hasMoved) {
        frameCount++;
        if (frameCount >= FRAME_LIMIT) {
            frameCount = 0;
            currentLoopIndex++;
            if (currentLoopIndex >= spriteFrames.length) {
                currentLoopIndex = 0;
            }
            var specMessage = `actionLoop Sprite: ${spritesDictionary[activeKey].name}\r\nSpecs: ${JSON.stringify(spriteFrames[currentLoopIndex].specs)}`;
            DotNet.invokeMethodAsync('CanvasAnimateRCL', 'MessageFromJs', specMessage);
        }
    }
    if (!hasMoved) {
        currentLoopIndex = 0;
    }
    spriteFrames = (faceCurrent === FACE_LEFT) ? spritesDictionary[activeKey].leftFrames : spritesDictionary[activeKey].rightFrames;
    image.src = spritesDictionary[activeKey].imgUrl;
    img.src = spritesDictionary[activeKey].imgUrl;
    var currentFrame = spriteFrames[currentLoopIndex].specs;
    width = currentFrame.w;
    height = currentFrame.h;
    scaledWidth = width * SCALE;
    scaledHeight = height * SCALE;
    drawFrame(currentFrame.x, currentFrame.y, positionX, positionY);
    requestId = window.requestAnimationFrame(actionLoop);
}
function drawBomb() {
    var frame = spritesDictionary["Bomb"].rightFrames[0].specs;

    ctx.drawImage(bombImg,
        frame.x, frame.y, frame.w, frame.h,
        canvas.width / 2, canvas.height / 2, 60, 60);
}
function initDead() {
   
    DotNet.invokeMethodAsync('CanvasAnimateRCL', 'MessageFromJs', 'Initialized deadLoop animation loop');
    image.src = spritesDictionary["Bomb"].imgUrl;
    img.src = spritesDictionary["Bomb"].imgUrl;
    spriteFrames = spritesDictionary["Bomb"].rightFrames;
    window.addEventListener('keydown', keyDownListener, false);
    function keyDownListener(event) {
        keyPresses[event.key] = true;
        console.log(`dead keydown: ${event.key}`);
    }

    window.addEventListener('keyup', keyUpListener, false);
    function keyUpListener(event) {
        keyPresses[event.key] = false;
        console.log(`keyup: ${event.key}`);
    }

    requestId = window.requestAnimationFrame(deadLoop);
}
function deadLoop() {
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    if (activeKey !== "Bomb" && activeKey !== "Dead") {
        activeKey = "Bomb";
    }
    spriteFrames = (faceCurrent === FACE_LEFT) ? spritesDictionary[activeKey].leftFrames : spritesDictionary[activeKey].rightFrames;
    if (keyPresses.enter) {
        reset('toInit');
    }
    frameCount++;
    if (frameCount >= 8) {
        frameCount = 0;
        currentLoopIndex++;
        if (currentLoopIndex >= spriteFrames.length) {
            if (activeKey === "Bomb") {
                activeKey = "Dead";
            } else if (activeKey === "Dead") {
                positionX = 0;
                positionY = 0;
                image.src = spritesDictionary[activeKey].imgUrl;
                img.src = spritesDictionary[activeKey].imgUrl;
                reset('toInit');
            }
            currentLoopIndex = 0;
        }
        var specMessage = `deadLoop Sprite: ${spritesDictionary[activeKey].name}\r\nSpecs: ${JSON.stringify(spriteFrames[currentLoopIndex].specs)}`;
        DotNet.invokeMethodAsync('CanvasAnimateRCL', 'MessageFromJs', specMessage);
    }
    if (!isDead) {
        return;
    }
    spriteFrames = (faceCurrent === FACE_LEFT) ? spritesDictionary[activeKey].leftFrames : spritesDictionary[activeKey].rightFrames;
    image.src = spritesDictionary[activeKey].imgUrl;
    img.src = spritesDictionary[activeKey].imgUrl;
   
    var frame = spriteFrames[currentLoopIndex].specs;

    ctx.drawImage(image,
        frame.x, frame.y, frame.w, frame.h,
        175, 175, 400, 400);
    
    requestId = window.requestAnimationFrame(deadLoop);
}
function drawFrame(frameX, frameY, canvasX, canvasY) {
    ctx.drawImage(image,
        frameX, frameY, width, height,
        canvasX, canvasY, scaledWidth, scaledHeight);
}
function moveCharacter(deltaX, deltaY) {
    if (positionX > canvas.width / 2 && positionX < canvas.width / 2 + 60) {
        activeKey = "Bomb";
        isDead = true;
        reset('toDead');
        //spriteFrames = (faceCurrent === FACE_LEFT) ? spritesDictionary[activeKey].leftFrames : spritesDictionary[activeKey].rightFrames;
        //image.src = spritesDictionary[activeKey].imgUrl;
        //img.src = spritesDictionary[activeKey].imgUrl;
        //requestId = window.requestAnimationFrame(deadLoop);
    }
    if (positionX + deltaX > 0 && positionX + scaledWidth + deltaX < canvas.width) {
        positionX += deltaX;
    }
    if (positionY + deltaY > 0 && positionY + scaledHeight + deltaY < canvas.height) {
        positionY += deltaY;
    }
}
export function reset(resetFunction) {
    if (requestId) {
        window.cancelAnimationFrame(requestId);
        requestId = null;
    }
    keyPresses = {};
    frameCount = 0;
    currentLoopIndex = 0;
    positionX = 0;
    positionY = 0;
    if (resetFunction === 'toDead') {
        initDead();
    } else {
        isDead = false;
        initAnimation(spritesDictionary);
    }
    
}

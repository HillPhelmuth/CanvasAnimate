const SCALE = 3;
let width = 16;
let height = 18;
let scaledWidth = SCALE * width;
let scaledHeight = SCALE * height;
const FRAME_LIMIT = 6;
const ACTION_LOOP = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];

let canvas = document.getElementById('canvasAnim');
let ctx = canvas.getContext('2d');
let keyPresses = {};
let currentLoopIndex = 0;
let frameCount = 0;

let img = document.getElementById('imgelem');
let attackImg = document.getElementById("attack");
let runImg = document.getElementById("run");
let jumpAttackImg = document.getElementById("jumpAttack");
let cowerImg = document.getElementById("idle");
let dotNetInstance = null;


//----------------------- Attack functions ------------------------------------------
export function initAction(instance) {
    dotNetInstance = instance;
    img = document.getElementById("run");
    console.log(JSON.stringify(img));
    height = 71;
    width = 59;
    scaledWidth = SCALE * width;
    scaledHeight = SCALE * height;
    window.addEventListener('keydown', keyDownListener, false);
    function keyDownListener(event) {
        keyPresses[event.key] = true;
    }

    window.addEventListener('keyup', keyUpListener, false);
    function keyUpListener(event) {
        keyPresses[event.key] = false;
    }
    window.requestAnimationFrame(actionLoop);
}
function actionLoop() {
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    let hasMoved = false;
    if (keyPresses.a) {
        img = cowerImg;
        hasMoved = true;
        console.log(JSON.stringify(img));
        dotNetInstance.invokeMethodAsync('HandleActionLog', 'cower ' + frameCount);
    } else if (keyPresses.d) {
        img = runImg;
        hasMoved = true;
        console.log(JSON.stringify(img));
        dotNetInstance.invokeMethodAsync('HandleActionLog', 'run ' + frameCount);
    } else if (keyPresses.s) {
        img = attackImg;
        hasMoved = true;
        console.log(JSON.stringify(img));
        dotNetInstance.invokeMethodAsync('HandleActionLog', 'attack ' + frameCount);
    } else if (keyPresses.w) {
        img = jumpAttackImg;
        hasMoved = true;
        console.log(JSON.stringify(img));
        dotNetInstance.invokeMethodAsync('HandleActionLog', 'jump attack ' + frameCount);
    }
    if (hasMoved) {
        frameCount++;
        if (frameCount >= FRAME_LIMIT) {
            frameCount = 0;
            currentLoopIndex++;
            if (currentLoopIndex >= ACTION_LOOP.length) {
                currentLoopIndex = 0;
            }
        }
    }
    if (!hasMoved) {
        currentLoopIndex = 0;
    }
    drawFrame(0, ACTION_LOOP[currentLoopIndex], 0, 0);
    window.requestAnimationFrame(actionLoop);
}
// -------------------------- Shared Functions -------------------------------

function drawFrame(frameX, frameY, canvasX, canvasY) {
    ctx.drawImage(img,
        frameX * width, frameY * height, width, height,
        canvasX, canvasY, scaledWidth, scaledHeight);
}



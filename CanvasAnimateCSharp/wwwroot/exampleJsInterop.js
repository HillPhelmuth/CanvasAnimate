// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.

export function showPrompt(message) {
  return prompt(message, 'Type anything here');
}
export function setEventListeners(instance) {
    window.addEventListener('keydown', keyDownListener, false);
    function keyDownListener(event) {
        let keyPressString = JSON.stringify(event.key);
        //console.log('keydown: ' + keyPressString);
        instance.invokeMethodAsync('HandleKeyDown', event.key);
    }

    window.addEventListener('keyup', keyUpListener, false);
    function keyUpListener(event) {
        let keyPressString = JSON.stringify(event.key);
        
        instance.invokeMethodAsync('HandleKeyUp', event.key);
    }
}

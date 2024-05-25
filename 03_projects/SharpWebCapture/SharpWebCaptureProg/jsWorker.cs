using OpenQA.Selenium;

namespace SharpWebCaptureProg
{
    internal class JsWorker
    {
        public void DefineSetBox01(IJavaScriptExecutor js)
        {
            var defineFun = @"
window.SetBox01 = function() {
    let box = document.querySelector('[aria-label^=""Wiadomości""]');
    box.classList.add(""box01"");
    window.box01 = document.getElementsByClassName('box01')[0];
};
";

            js.ExecuteScript(defineFun);
        }

        public void DefineGetDeepestNestedDiv(IJavaScriptExecutor js)
        {
            var defineFun= @"
window.getDeepestNestedDiv = function getDeepestNestedDiv(rootDiv) {
    if (rootDiv.tagName !== 'DIV') {
        throw new Error('Provided element is not a DIV.');
    }

    let deepestDiv = rootDiv;
    let maxDepth = 0;

    function traverse(element, depth) {
        if (element.tagName === 'DIV') {
            if (depth > maxDepth) {
                maxDepth = depth;
                deepestDiv = element;
            }
        }

        for (let i = 0; i < element.children.length; i++) {
            traverse(element.children[i], depth + 1);
        }
    }

    traverse(rootDiv, 0);

    return deepestDiv;
}
";

            js.ExecuteScript(defineFun);
        }

        public void DefineSelectByStyle(IJavaScriptExecutor js)
        {
            var defineFun = @"
(function() {
  var elms = document.querySelectorAll(""*[style]"");
  Array.prototype.forEach.call(elms, function(elm) {

    var clr = elm.style.color || """";

    // Remove all whitespace, make it all lower case
    clr = clr.replace(/\s/g, """").toLowerCase();

    // Switch on the possible values we know of
    switch (clr) {
      case ""#333"":
      case ""#333333"":
      case ""rgb(51,51,51)"": // <=== This is the one Chrome seems to use
      case ""rgba(51,51,51,0)"":
        elm.style.color = ""#444"";
        break;
    }
  });
})();
";

            js.ExecuteScript(defineFun);
        }
    }
}

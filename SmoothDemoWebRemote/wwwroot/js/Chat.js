// The following sample code uses modern ECMAScript 6 features 
// that aren't supported in Internet Explorer 11.
// To convert the sample for environments that do not support ECMAScript 6, 
// such as Internet Explorer 11, use a transpiler such as 
// Babel at http://babeljs.io/. 
//
// See Es5-chat.js for a Babel transpiled version of the following code:

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();

connection.on("ReceiveMessage", (user, message) => {
    const msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    const encodedMsg = user + " requested " + msg;
    const li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

function enact(what) {
    var p = what.parentNode;
    var els = p.getElementsByTagName('li');
    //for (i = 0; i < els.length; i++) {
    //    els[i].className = '';
    //}
    what.className = 'active';
}



connection.on("ReceiveActionList", (actionList) => {
    console.log(actionList);

    var myArr = JSON.parse(actionList);



    var $ul = $('#actionList');
    $ul.empty();
    $.each(myArr, function (index, item) {
        if (item.Type == 'Run') {
            $ul.append('<li onclick="enact(this);" id="item' + index + '" style="margin:5px;"> <input class="btn" style="margin:5px;" type="button" value="RUN" onclick="Execute(' + index + ' );" /><h3>' + item.Content + '</h3></li>')
        }
        if (item.Type == 'Audio') {
            $ul.append('<li onclick="enact(this);" id="item' + index + '" style="margin:5px;color:blue"> <input class="btn warning" style="margin:5px;" type="button" value="AUDIO" onclick="Execute(' + index + ' );" /><h3>' + item.Content + '</h3></li>')
        }
        else if (item.Type == 'Show') {
            $ul.append('<li style="margin:5px;" onclick="enact(this);" ><h3>' + item.Content + '</h3></li>')
        }
        else if (item.Type == 'Input') {
            $ul.append('<li onclick="enact(this);" id="item' + index + '" style="margin:5px;"> <input class="btn" style="margin:5px;" type="button" value="INPUT" onclick="Execute(' + index + ' );" /><h3>' + item.Content + '</h3></li>')

        }
        else if (item.Type == 'Section') {
            $ul.append('<li style="margin:10px;font-size: 40px;color:red" onclick="enact(this);" ><h1>' + item.Content + '</h1></li>')

        }
    })
    //const msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    //const encodedMsg = user + " says " + msg;
    //const li = document.createElement("li");
    //li.textContent = encodedMsg;
    //document.getElementById("messagesList").appendChild(li);
});




connection.start().catch(err => console.error(err.toString()));

document.getElementById("sendButton").addEventListener("click", event => {
    const user = document.getElementById("userInput").value;
    const message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(err => console.error(err.toString()));
    event.preventDefault();
});

document.getElementById("requestActionListButton").addEventListener("click", event => {

    connection.invoke("RequestActionList").catch(err => console.error(err.toString()));
    event.preventDefault();
});

function Execute(index) {
    connection.invoke("SendMessage", "webremote", "execute:" + index).catch(err => console.error(err.toString()));
}
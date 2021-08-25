function nonAccentVietnamese(str) {
    str = str.toLowerCase();
    str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
    str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
    str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
    str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
    str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
    str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
    str = str.replace(/đ|ð/g, "d");
    str = str.replace(/\u0300|\u0301|\u0303|\u0309|\u0323/g, "");
    str = str.replace(/\u02C6|\u0306|\u031B/g, "");
    return str;
}

function autocomplete(inp, arr) {

    function closeAllLists(elmnt) {
        var x = document.getElementsByClassName("autocomplete-items");
        for (var i = 0; i < x.length; i++) {
            if (elmnt !== x[parseInt(i, 10)] && elmnt !== inp) {
                x[parseInt(i, 10)].parentNode.removeChild(x[parseInt(i, 10)]);
            }
        }
    }

    var currentFocus;

    if (inp) {
        inp.oninput = function () {
            var a, b, c, i, val = this.value;

            closeAllLists();
            if (!val) { return false; }
            currentFocus = -1;

            a = document.createElement("DIV");
            a.setAttribute("id", this.id + "autocomplete-list");
            a.setAttribute("class", "autocomplete-items");

            this.parentNode.appendChild(a);

            for (i = 0; i < arr.length; i++) {
                var inputValue = nonAccentVietnamese(val);
                var listItem = nonAccentVietnamese(arr[parseInt(i, 10)]);

                if (listItem.includes(inputValue)) {
                    b = document.createElement("DIV");
                    var emphasize = "<strong>" + arr[parseInt(i, 10)] + "</strong>";
                    emphasize += "<input type='hidden' value='" + arr[parseInt(i, 10)] + "'>";
                    b.innerHTML = emphasize;
                    b.onclick = function () {
                        inp.value = this.getElementsByTagName("input")[0].value;
                        closeAllLists();
                    };
                    a.appendChild(b);
                }
            }
            c = document.createElement("DIV");
            c.innerHTML = "<strong>" + "Other" + "</strong>";
            c.innerHTML += "<input type='hidden' value='Other'>";
            c.onclick = function () {
                inp.value = this.getElementsByTagName("input")[0].value;
                closeAllLists();
            };
            a.appendChild(c);
        };
    }

    function removeActive(x) {
        for (var i = 0; i < x.length; i++) {
            x[parseInt(i, 10)].classList.remove("autocomplete-active");
        }
    }

    function addActive(x) {
        if (!x) { return false; }
        removeActive(x);
        if (currentFocus >= x.length) { currentFocus = 0; }
        if (currentFocus < 0) { currentFocus = (x.length - 1); }
        x[parseInt(currentFocus, 10)].classList.add("autocomplete-active");
    }

    if (inp) {
        inp.onkeydown = function (e) {
            var x = document.getElementById(this.id + "autocomplete-list");
            if (x) { x = x.getElementsByTagName("div"); }
            if (e.keyCode === 40) {
                currentFocus++;
                addActive(x);
            } else if (e.keyCode === 38) {
                currentFocus--;
                addActive(x);
            } else if (e.keyCode === 13) {
                e.preventDefault();
                if (currentFocus > -1) {
                    if (x) { x[parseInt(currentFocus, 10)].click(); }
                }
            }
        };
    }

    document.onclick = function (e) {
        closeAllLists(e.target);
    };
}

autocomplete();
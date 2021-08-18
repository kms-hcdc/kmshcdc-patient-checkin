function autocomplete(inp, arr) {

    function closeAllLists(elmnt) {
        var x = document.getElementsByClassName("autocomplete-items");
        for (var i = 0; i < x.length; i++) {
            if (elmnt !== x[parseInt(i)] && elmnt !== inp) {
                x[parseInt(i)].parentNode.removeChild(x[parseInt(i)]);
            }
        }
    }

    var currentFocus;

    inp.addEventListener("input", function () {
        var a, b, c, i, val = this.value;

        closeAllLists();
        if (!val) { return false; }
        currentFocus = -1;

        a = document.createElement("DIV");
        a.setAttribute("id", this.id + "autocomplete-list");
        a.setAttribute("class", "autocomplete-items");

        this.parentNode.appendChild(a);

        for (i = 0; i < arr.length; i++) {
            if (arr[parseInt(i)].substr(0, val.length).toUpperCase() === val.toUpperCase()) {
                b = document.createElement("DIV");
                b.innerHTML = "<strong>" + arr[parseInt(i)].substr(0, val.length) + "</strong>";
                b.innerHTML += arr[parseInt(i)].substr(val.length);
                b.innerHTML += "<input type='hidden' value='" + arr[parseInt(i)] + "'>";
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
    });

    function removeActive(x) {
        for (var i = 0; i < x.length; i++) {
            x[parseInt(i)].classList.remove("autocomplete-active");
        }
    }

    function addActive(x) {
        if (!x) { return false; }
        removeActive(x);
        if (currentFocus >= x.length) { currentFocus = 0; }
        if (currentFocus < 0) { currentFocus = (x.length - 1); }
        x[parseInt(currentFocus)].classList.add("autocomplete-active");
    }

    inp.addEventListener("keydown", function (e) {
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
                if (x) { x[parseInt(currentFocus)].click(); }
            }
        }
    });

    document.addEventListener("click", function (e) {
        closeAllLists(e.target);
    });
}

autocomplete();
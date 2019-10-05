let app = new Vue({
    el: "#app",
    data: {
        display: null,
        state: {board: ""},
        err: ""
    },
    mounted() {
        let options = {
            forceSquareRatio: true
        };
        this.display = new ROT.Display(options);
        document.getElementById("game").replaceWith(this.display.getContainer());
        this.getData();
    },
    methods: {
        draw: function (board) {
            console.log("drawing board:\n", this.board);
            this.display.drawText(0, 0, this.state.board);
            if (this.state.playerLocation) {
                this.display.drawText(this.state.playerLocation.x, this.state.playerLocation.y, "@")
            }
        },
        getData: function () {
            axios.get("/map")
                .then(resp => {
                    console.log("Got data:\n", resp);
                    this.state = resp.data;
                    this.draw();
                })
                .catch(err => this.err = err);
        },
        // sendCommand will be wired to the various keybinds
        // each binding should send the game command expected
        sendCommand: function (direction) {
            let move = {"case": "Move", "fields": [{"case": direction}]}
            axios.post("/cmd", move)
                .then(resp => {
                    console.log("Got data from cmd:\n", resp)
                    this.state = resp.data;
                    this.draw()
                })
                .catch(err => this.err = err);
        }
    }
});

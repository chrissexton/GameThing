let app = new Vue({
    el: "#game" ,
    data: {
        display: null,
        board: [],
        err: ""
    },
    mounted() {
        let options = {
            forceSquareRatio:true
        };
        this.display = new ROT.Display(options);
        document.getElementById("game").appendChild(this.display.getContainer());
        this.getData();
    },
    methods: {
        draw: function(board) {
            console.log("drawing board:\n", this.board);
            // let str = "Using a regular grid\n@....%b{blue}#%b{}##.%b{red}.%b{}.$$$";
            this.display.drawText(0,  0, this.board);
        },
        getData: function() {
            axios.get("/map")
                .then(resp => {
                    console.log("Got data:\n", resp);
                    this.board = resp.data;
                    this.draw();
                })
                .catch(err => this.err = err);
        }
    }
});

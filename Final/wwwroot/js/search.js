new Vue({
    el: '#app',
    data: {
        currentPage: 1,
        tires: [],
        wheels: [],
        totalPages: 0,
        tireForm: {
            season: '',
            width: '',
            height: '',
            diameter: ''
        },
        wheelForm: {
            width: '',
            diameter: '',
            hole: '',
            holeDiameter: ''
        }
    },
    methods: {
        fetchTires: function(page){
            let options = {
                params:{
                    page: page,
                    season: this.tireForm.season,
                    width: this.tireForm.width,
                    height: this.tireForm.height,
                    diameter: this.tireForm.diameter,
                }
            };
            this.$http.get('https://localhost:5001/api/Main/tires', options).then(
                function (response) {
                    this.tires = response.body.products;
                    this.tires.forEach(t => t.image = "img/tire/" + t.image);
                    this.totalPages = response.body.paginationModel.totalPages;
                    this.currentPage = response.body.paginationModel.pageNumber;
                    console.log(response.body)
                }, console.log
            )
        },
        fetchWheels: function(page){
            let options = {
                params:{
                    page: page,
                    width: this.wheelForm.width,
                    hole: this.wheelForm.hole,
                    holeDiameter: this.wheelForm.holeDiameter,
                    diameter: this.wheelForm.diameter,
                }
            };
            this.$http.get('https://localhost:5001/api/Main/wheels', options).then(
                function (response) {
                    this.wheels = response.body.products;
                    this.wheels.forEach(w => w.image = "img/wheel/" + w.image);
                    this.totalPages = response.body.paginationModel.totalPages;
                    this.currentPage = response.body.paginationModel.pageNumber;
                    console.log(response.body)
                }, console.log
            )
        }
    }
});
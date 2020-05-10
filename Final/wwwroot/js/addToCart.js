new Vue({
    el: '#app',
    data: {
        error: false,
        success: false,
        quantity: ''
    },
    methods: {
        addToCart: function(event){
            this.$http.post('https://localhost:5001/api/Main/addtocart', {
                productId: event.target.getAttribute('product-id').valueOf(),
                categoryId: event.target.getAttribute('category-id').valueOf()
            }).then(
                function (response) {
                    this.success = true;
                    this.error = false;
                    this.quantity = response.body.quantity;
                }
            ).catch(
                function () {
                    this.success = false;
                    this.error = true;
            })
        },
    }
});
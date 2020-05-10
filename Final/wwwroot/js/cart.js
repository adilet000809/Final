new Vue({
    el: '#cart',
    data: {
        cartItems: '',
        error: false,
        errorDescription: '',
        empty: false,
        total: 0
    },
    methods: {
        fetchCart: function(){
            this.$http.get('https://localhost:5001/api/Main/cart').then(
                function (response) {
                    if(response.body.length>0){
                        this.empty = false;
                        this.cartItems = response.body;
                        for(let i=0; i<response.body.length; i++){
                            this.total += this.cartItems[0].product.price * this.cartItems[0].quantity;
                        }
                    }
                    else {
                        this.empty = true;
                    }
                }
            ).catch(
                function (response) {
                    console.log(response);
                })
        },
        deleteCartItem: function (event) {
            let options = {
                params: {
                    cartItemId: event.target.getAttribute('item-id').valueOf()
                }
            }
            
            this.$http.delete('https://localhost:5001/api/Main/cartitem/' + event.target.getAttribute('item-id').valueOf())
                .then(
                    function (response) {
                        this.error = false;
                        this.fetchCart();
                    }
                ).catch(
                    function (response) {
                        this.errorDescription = response.body;
                        this.error = true;
                    })
        }
    },
    created: function () {
        this.fetchCart();
    }
});
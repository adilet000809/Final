Vue.component('pagination', {
    template: '#pagination-template',
    props: {
        current: {
            type: Number,
            default: 1
        },
        totalPages: {
            type: Number,
            default: 0
        },
        pageRange: {
            type: Number,
            default: 2
        }
    },
    computed: {
        pages: function() {
            let pages = [];

            for(let i = this.rangeStart; i <= this.rangeEnd; i++) {
                pages.push(i)
            }

            return pages
        },
        rangeStart: function() {
            let start = this.current - this.pageRange;

            return (start > 0) ? start : 1
        },
        rangeEnd: function() {
            let end = this.current + this.pageRange;

            return (end < this.totalPages) ? end : this.totalPages
        },
        nextPage: function() {
            return this.current + 1
        },
        prevPage: function() {
            return this.current - 1
        }
    },
    methods: {
        hasFirst: function() {
            return this.rangeStart !== 1
        },
        hasLast: function() {
            return this.rangeEnd < this.totalPages
        },
        hasPrev: function() {
            return this.current > 1
        },
        hasNext: function() {
            return this.current < this.totalPages
        },
        changePage: function(page) {
            this.$emit('page-changed', page)
        }
    }
});
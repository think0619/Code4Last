<template>
    <div> 
    </div>
</template>


<script setup> 
</script>

<style lang="scss" scoped>

</style>

<script>
export default {
    components: {
    },
    data() { 
         return {
            title:'生态治理',
            active: 0,
        };
    },
    computed: {
    },
    setup() {
    },
    mounted() {
        this.$emit("onChangeTitle", this.title);
    },
    methods: {
        onClickLeft2() {
            // this.$parent.onClickLeft();
             this.$emit('onClickLeftChild',title); 
        },
    }
}; 
</script>
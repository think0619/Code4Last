<template>
    <div>
        tab3

        <div @click="goto('/home')">跳转首页</div>
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
            title:'绿色低碳智慧园区',
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
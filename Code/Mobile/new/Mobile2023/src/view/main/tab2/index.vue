<template>
    <div>
        <h>{{ title }}</h>
    <div>
        <van-button type="primary" size="normal" block class="cusbtn" @click="onClickCmdBtn('page2_btn1')">暂停</van-button>
        <van-button type="primary" size="normal" block class="cusbtn" @click="onClickCmdBtn('page2_btn2')">启动</van-button>
        <van-button type="primary" size="normal" block class="cusbtn" @click="onClickCmdBtn('page2_btn3')">单个循环</van-button>
        <van-button type="primary" size="normal" block class="cusbtn" @click="onClickCmdBtn('page2_btn4')">列表循环</van-button>
    </div>
    </div>
</template>


<script setup> 


</script>

<script>
export default {
    components: {
    },
    data() { 
         return {
            title:'智能井口',
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
        onClickCmdBtn(btnkey){ 
            this.$emit('onDoCMDSend',btnkey); 
            
        }
    }
}; 
</script>
<style lang="scss" scoped>

</style>
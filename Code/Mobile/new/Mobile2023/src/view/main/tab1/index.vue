<template>
    <h>{{ title }}</h>
    <div>
        <van-button type="primary" size="normal" block class="cusbtn" @click="onClickCmdBtn('page1_btn1')">视频1</van-button>
        <van-button type="primary" size="normal" block class="cusbtn" @click="onClickCmdBtn('page1_btn2')">视频2</van-button>
        <van-button type="primary" size="normal" block class="cusbtn" @click="onClickCmdBtn('page1_btn3')">视频3</van-button>
        <van-button type="primary" size="normal" block class="cusbtn" @click="onClickCmdBtn('page1_btn4')">视频4</van-button>
    </div> 
</template> 
<style scoped>
h {
    color: aliceblue;
}
</style>

<script>
 

export default {
    components: {
    },
    data() {
        return {
            title: '智能喷涂',
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
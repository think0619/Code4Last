<template>
    <div class="contentTitle">
        <span>{{ contentTitle }}</span>
    </div>
    <div class="btnArea">
        <van-button type="primary" size="normal" block class="cmdCustomBtn1"
            @click="onClickCmdBtn('page1_btn1')">视频1</van-button>
        <van-button type="primary" size="normal" block class="cmdCustomBtn1"
            @click="onClickCmdBtn('page1_btn2')">视频2</van-button>
        <van-button type="primary" size="normal" block class="cmdCustomBtn1"
            @click="onClickCmdBtn('page1_btn3')">视频3</van-button>
        <van-button type="primary" size="normal" block class="cmdCustomBtn1"
            @click="onClickCmdBtn('page1_btn4')">视频4</van-button> 

    </div>
    <van-image round class="cmdResetBtn" fit="cover" 
         @click="onclickReset('page1_reset')"
        :src="resetBtnImg" />
</template> 
 
<style scoped></style>

<script setup lang="jsx"> 
import resetBtnImg from '@/assets/img/setting.svg'
</script>

<script lang="jsx">
export default {
    components: {
    },
    data() {
        return {
            title: '智能喷涂',
            contentTitle: '智能喷涂',
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
        onClickCmdBtn(btnkey) {
            this.$emit('onDoCMDSend', btnkey);
        },
        onclickReset(btnkey) {
            this.$emit('onDoCMDSend', btnkey);
            
        }
    }
}; 
</script>
<template>
    <!-- <div class="contentTitle"> 
        <span>{{ contentTitle }}</span>
    </div> -->
    <div class="btnArea" >
        <!-- <van-button type="primary" size="normal" block class="cmdCustomBtn1  " @click="onClickCmdBtn('page2_btn1')">视频1</van-button>
        <van-button type="primary" size="normal" block class="cmdCustomBtn1  " @click="onClickCmdBtn('page2_btn2')">视频2</van-button>
        <van-button type="primary" size="normal" block class="cmdCustomBtn1" @click="onClickCmdBtn('page2_btn3')">视频3</van-button>
        <van-button type="primary" size="normal" block class="cmdCustomBtn1" @click="onClickCmdBtn('page2_btn4')">视频4</van-button>  -->
    </div>
    <van-image round class="cmdResetBtn" fit="cover" 
         @click="onclickReset('page2_reset')"
        :src="resetBtnImg" />

</template> 
<script setup lang="jsx"> 
import resetBtnImg from '@/assets/img/setting.svg'
</script>
<script lang="jsx"> 
export default {
    components: {
    },
    data() { 
         return {
            title:'绿色矿山展区',
            contentTitle:'智能井口',
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


<style> 
.content {
   background: url(../../../assets/img/back/b2.jpg);
   background-repeat: no-repeat;
   background-size: 100vw 100vh;
 } 
</style>
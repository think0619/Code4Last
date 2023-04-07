<template>
    <div class="contentTitle">
        <span>{{ contentTitle }}</span>
    </div>
    <div class="btnArea">
        <van-space direction="vertical" fill size="2vh">
            <div class="cmdCustomBtn1 btn1 " @click="onClickCmdBtn('page4_btn1')">
                <span class="btn_name">智慧园区</span>
                <van-image round class="crtIcon" fit="cover" :src="correcticon" v-show="showlist.page4_btn1" />
            </div>
            <div class="cmdCustomBtn1 btn1" @click="onClickCmdBtn('page4_btn2')">
                <span class="btn_name">智慧园区</span>
                <van-image round class="crtIcon" fit="cover" :src="correcticon" v-show="showlist.page4_btn2" />
            </div>
            <div class="cmdCustomBtn1 btn1" @click="onClickCmdBtn('page4_btn3')">
                <span class="btn_name">智慧园区</span>
                <van-image round class="crtIcon" fit="cover" :src="correcticon" v-show="showlist.page4_btn3" />
            </div>
            <div class="cmdCustomBtn1 btn1" @click="onClickCmdBtn('page4_btn4')">
                <span class="btn_name">智慧园区</span>
                <van-image round class="crtIcon" fit="cover" :src="correcticon" v-show="showlist.page4_btn4" />
            </div>
        </van-space>
        <van-image round class="cmdResetBtn" fit="cover" @click="onclickReset('page4_reset')" :src="resetBtnImg" />
    </div>
</template> 
 
<style scoped>
.btn1 {
    background: url(../../../assets/img/p4/e1.png);
    background-repeat: no-repeat;
    background-size: cover;
}  
</style>

<style> 
.content {
   background: url(../../../assets/img/back/b1.jpg) !important;
   background-size: cover;
   height: 100%;
 } 
</style>

<script setup lang="jsx">
import resetBtnImg from '@/assets/img/setting.svg';
import correcticon from '@/assets/img/icons8-checkmark.svg';
import p4r1 from '@/assets/img/p4/e1.png';
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
            show: false,
            showlist: {
                page4_btn1: false,
                page4_btn2: false,
                page4_btn3: false,
                page4_btn4: false,
            }
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
            for (let key in this.showlist) {
                if (key == btnkey) {
                    this.showlist[key] = true;
                }
            }
        },
        onclickReset(btnkey) {
            this.$emit('onDoCMDSend', btnkey);
            for (let key in this.showlist) {
                this.showlist[key] = false;
            }
        }
    }
}; 
</script>
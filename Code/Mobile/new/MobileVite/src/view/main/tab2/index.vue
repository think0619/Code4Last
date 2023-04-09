<template>
    <div class="contentTitle"> 
        <span>{{ contentTitle }}</span>
    </div>
    <div class="btnArea">
        <van-space direction="vertical" fill size="2vh">
            <div class="cmdCustomBtn1 cmdbtn1-1" @click="onClickCmdBtn('page2_btn1')">
                <span class="btn_name">暂停</span>
                <van-image round class="crtIcon" fit="cover" :src="correcticon" v-show="showlist.page2_btn1" />
            </div>
            <div class="cmdCustomBtn1 cmdbtn1-2" @click="onClickCmdBtn('page2_btn2')">
                <span class="btn_name">开始</span>
                <van-image round class="crtIcon" fit="cover" :src="correcticon" v-show="showlist.page2_btn2" />
            </div>
            <div class="cmdCustomBtn1 cmdbtn1-3" @click="onClickCmdBtn('page2_btn3')">
                <span class="btn_name">智慧园区3</span>
                <van-image round class="crtIcon" fit="cover" :src="correcticon" v-show="showlist.page2_btn3" />
            </div>
            <div class="cmdCustomBtn1 cmdbtn1-4" @click="onClickCmdBtn('page2_btn4')">
                <span class="btn_name">智慧园区4</span>
                <van-image round class="crtIcon" fit="cover" :src="correcticon" v-show="showlist.page2_btn4" />
            </div>
            <div class="cmdCustomBtn1 cmdbtn1-4" @click="onClickCmdBtn('page2_btn5')">
                <span class="btn_name">智慧园区1</span>
                <van-image round class="crtIcon" fit="cover" :src="correcticon" v-show="showlist.page2_btn5" />
            </div>
            <div class="cmdCustomBtn1 cmdbtn1-4" @click="onClickCmdBtn('page2_btn6')">
                <span class="btn_name">智慧园区2</span>
                <van-image round class="crtIcon" fit="cover" :src="correcticon" v-show="showlist.page2_btn6" />
            </div>
        </van-space>
        <van-image round class="cmdResetBtn" fit="cover" @click="onclickReset('page2_reset')" :src="resetBtnImg" />
    </div>
</template> 
<script setup lang="jsx"> 
import resetBtnImg from '@/assets/img/resetbtn.png';
import correcticon from '@/assets/img/icons8-checkmark.svg';
</script>
<script lang="jsx"> 
export default {
    components: {
    },
    data() { 
         return {
            title:'绿色矿山展区',
            contentTitle:'绿色展区主屏幕',
            active: 0,
             showlist: {
                page2_btn1: false,
                page2_btn2: false,
                page2_btn3: false,
                page2_btn4: false,
                page2_btn5: false,
                page2_btn6: false,
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

 
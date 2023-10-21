<template>
    <div class="content">
        <!-- <van-nav-bar class="vannav" :border=false :title="contentTitle" left-text="" left-arrow @click-left="onClickLeft" /> -->
        <div class="contentTitle">
            <span>{{ contentTitle }}</span>
        </div>
        <div class="btnArea">
            <van-space direction="vertical" fill size="2vh"> 
                <div v-for="(item, index) in btnlist"  :key="index"  :id="item.btnid"  class="cmdCustomBtn1" :class="item.classname" @click="sendmsg(item.btnid, item.msg)">
                    <span class="btn_name">{{ item.fieldtext }}</span>
                    <van-image round class="crtIcon" fit="cover" :src="correcticon" v-show="item.showarrow" />
                </div>  
            </van-space>  
        </div>
        <van-action-bar safe-area-inset-bottom>
            <van-action-bar-icon :icon="customicons.replay" text="重置" @click="clickcontrol('reset')" />
            <van-action-bar-icon :icon="customicons.play" text="开始" @click="clickcontrol('start')" />
            <van-action-bar-icon :icon="customicons.stop" text="暂停" @click="clickcontrol('pause')" />
            <van-action-bar-button type="warning" color="linear-gradient(to bottom right,#329eff, #4969f3)" text="上一个" @click="clickcontrol('previous')" />
            <van-action-bar-button type="primary" color="linear-gradient(to bottom right,#57abf6, #36b4fc)" text="下一个" @click="clickcontrol('next')" />
        </van-action-bar> 
    </div>
</template>   

<script setup lang="jsx">
import { showSuccessToast, showFailToast, showToast } from 'vant';
import correcticon from '@/assets/img/icons8-checkmark.svg';


import { login } from "@/api/config";
import { getMQTTInfo } from "@/api/mqtthelper";
import * as mqtt from "mqtt/dist/mqtt.min";
</script> 

<script lang="jsx">
import mediaPlayIcon from '@/assets/img/media/icons8-play-64.png';
import mediaForwardIcon from '@/assets/img/media/icons8-fast-forward-64.png';
import mediaRewindIcon from '@/assets/img/media/icons8-rewind-64.png';
import mediaStopIcon from '@/assets/img/media/icons8-stop-64.png';
import mediaReplayIcon from '@/assets/img/media/icons8-replay-64.png';
import resetBtnImg from '@/assets/img/resetbtn.png';

export default {
    components: {
    },
    data() {
        return {
            contentTitle: 'CCTEG · 国之担当',
            active: 0,
            show: false,
            curBtnIndex: -1,
            // scrolltop:0,
            pageBtnArrayKey: ['page1_btn1', 'page1_btn2', 'page1_btn3', 'page1_btn4',],
            showlist: {
                page1_btn1: false,
                page1_btn2: false,
                page1_btn3: false,
                page1_btn4: false,
                page1_btn5: false,
                page1_btn6: false,
                page1_btn7: false,
                page1_btn8: false,
            },
            customicons: {
                forward: mediaForwardIcon,
                rewind: mediaRewindIcon,
                stop: mediaStopIcon,
                play: mediaPlayIcon,
                reset: resetBtnImg,
                replay: mediaReplayIcon
            },
            btnlist:[
                {btnid:"page1_btn1",classname:"cmdbtn1-1",msg:"video_pv01",fieldtext:"功能按键-1",showarrow:false},
                {btnid:"page1_btn2",classname:"cmdbtn1-2",msg:"video_pv02",fieldtext:"功能按键-2",showarrow:false},
                {btnid:"page1_btn3",classname:"cmdbtn1-3",msg:"video_pv03",fieldtext:"功能按键-3",showarrow:false},
                {btnid:"page1_btn4",classname:"cmdbtn1-4",msg:"video_pv04",fieldtext:"功能按键-4",showarrow:false},
                {btnid:"page1_btn5",classname:"cmdbtn2-1",msg:"video_pv05",fieldtext:"功能按键-5",showarrow:false},
                {btnid:"page1_btn6",classname:"cmdbtn2-2",msg:"video_pv06",fieldtext:"功能按键-6",showarrow:false},
                {btnid:"page1_btn7",classname:"cmdbtn2-3",msg:"video_pv07",fieldtext:"功能按键-7",showarrow:false},
                {btnid:"page1_btn8",classname:"cmdbtn2-4",msg:"video_pv08",fieldtext:"功能按键-8",showarrow:false},
            ] 
        };
    },
    computed: {
    },
    setup() {

    },
    mounted() {
        //  this.$emit("onChangeTitle", this.title);
        const idcode = this.$route.query.idcode;
        if (idcode != null) {
            login(idcode).then((reqresult) => {
                if (reqresult.data) {
                    let result = reqresult.data; //Msg Token 
                    if (result.Status == 1) {
                        var token = result.Token;
                        //get mqtt info 
                        if (token != null) {
                            showSuccessToast({
                                "wordBreak": "break-word",
                                "message": "Authenticated.",
                                "duration": 800
                            });
                            getMQTTInfo(token).then((mqttr) => {
                                if (mqttr != null && mqttr.Status == 1) {
                                    let mqttdata = mqttr.Data;
                                    let mqtt_wsurl = mqttdata.wsurl;
                                    let mqtt_username = mqttdata.username;
                                    let mqtt_password = mqttdata.password;
                                    this.connectMQTT(mqtt_wsurl, mqtt_username, mqtt_password)
                                } else {
                                    showFailToast({
                                        "wordBreak": "break-word",
                                        "message": "认证过期，刷新页面.",
                                        "duration": 800
                                    });
                                }
                            });
                        }
                    } else {
                        showFailToast({
                            "wordBreak": "break-word",
                            "message": "认证失败." + result.Msg,
                            "duration": 800
                        });
                    }
                } else {
                    showFailToast('login error')
                }
            })
        } else {
            showFailToast('The url is error.')
        }

    },
    methods: {
        connectMQTT(_url, _username, _password) {
            let that = this;

            let mqttid = that.$store.state.mqttid;
            if (!mqttid) {
                let _mqttid = 'ExibitionMobile_' + Math.random().toString(16).substr(2, 8);
                that.$store.commit('setToken', _mqttid)
                console.log(that.$store.state.mqttid)
            }

            that.mqttclient = mqtt.connect(_url, {
                clientId: that.$store.state.mqttid,
                keepalive: 60,
                reconnectPeriod: 30000,
                username: _username,
                password: _password
            })
            that.mqttclient.on('connect', function () {
                // showSuccessToast({
                //     "wordBreak": "break-word",
                //     "message": "The mqtt client has connected.",
                //     "duration": 800
                // });
                // that.mqttclient.subscribe('ShowClockTime', function (err) {
                //     if (!err) { } else { }
                // }) 
            })
            that.mqttclient.on('message', function (topic, message) {
                // console.log(topic.toString())
                // console.log(message.toString())
            })
        },

        publicmqtt(msg,fieldtext) {
            //cmd$Beijing${GUID}_restartapp 
            //cmd$Beijing${GUID}_video_pv01
            //cmd$Beijing${GUID}_video_pause
            //cmd$Beijing${GUID}_video_start
            //cmd$Beijing${GUID}_video_single
            //cmd$Beijing${GUID}_video_list 
            //cmd$Beijing${GUID}_video_next 
            //cmd$Beijing${GUID}_video_previous

            if (msg.length > 0) {
                const topic = "ExhibitionPC"
                const GUID = 'YUIDX43';
                var fullmsg = "cmd$Beijing$" + GUID + "_" + msg;
                let that = this;
                if (that.mqttclient != null) {
                    that.mqttclient.publish(topic, fullmsg, {
                        qos: 0,
                        retain: false
                    }, function (err) {
                        if (err) {
                            showToast({
                                "wordBreak": "break-word",
                                "message": "发送失败，" + err,
                                "duration": 800,
                                'icon': 'fail'
                            })
                        } else {
                            showToast({
                                "wordBreak": "break-word",
                                "message": "发送成功."+fieldtext,
                                "duration": 800,
                                'icon': 'success'
                            })
                        }
                    })
                } else {
                    showToast({
                        "wordBreak": "break-word",
                        "message": "The mqtt client has disconnected.",
                        "duration": 800,
                        'icon': 'fail'
                    })
                }
            }
        },

        sendmsg(btnkey, msg) {
            let that = this;
            // for (let key in that.showlist) {
            //     if (key == btnkey) {
            //         that.showlist[key] = true;
            //     } else {
            //         that.showlist[key] = false;
            //     }
            // } 
            let btnfiledText = ""; 
            that.btnlist.forEach(function (ele) {
                if (ele.btnid === btnkey) {
                    btnfiledText = ele.fieldtext;return;
                } 
            });
            this.publicmqtt(msg, btnfiledText);
        },

        clickcontrol(name) {
            let msg = "";
            switch (name) {
                case "reset": msg = "video_pv01"; break;
                case "start": msg = "video_start"; break;
                case "pause": msg = "video_pause"; break;
                case "previous": msg = "video_previous"; break;
                case "next": msg = "video_next"; break;
            } 
            if(msg) {
            this.publicmqtt(msg,"");
        }

    }
}
}; 
</script>



<style>
:root:root {
    --van-nav-bar-title-text-color: #fff;
    --van-nav-bar-icon-color: #fff;
    --van-nav-bar-arrow-size: 20px;

    --van-tabbar-height: 60px;
    --van-tabbar-background: rgba(13, 37, 111, 0.800);

    --van-tabbar-item-active-background: transparent;
    --van-tabbar-item-icon-size: 24px;
    --van-tabbar-item-font-size: 16px;
    --van-tabbar-item-text-color: #FFF;
    --van-tabbar-item-active-color: #d1ccccbd;

    --van-action-bar-background: rgba(13, 37, 111, 0.800);
    --van-action-bar-icon-background: rgba(13, 37, 111, 0.800);
    --van-action-bar-height: 60px;
    --van-action-bar-icon-width: 55px;
    --van-action-bar-icon-height: 100%;
    --van-action-bar-icon-size: 20px;
    --van-action-bar-icon-font-size: 16px;
    --van-action-bar-icon-text-color: #FFF;
    --van-action-bar-button-height:40px

}

.van-nav-bar {
    /* background: url(../../assets/img/navbarbg.png); */
    background-color: rgba(13, 37, 111, 0.800);
    background-repeat: no-repeat;
    background-size: 100vw 100vh;
    --van-nav-bar-height: 6vh
}

.content {
    height: 100%;
    background: url(@/assets/img/back/itembg.png);
    background-repeat: no-repeat;
    background-size: 100vw 100vh;
}

.contentTitle {
    width: 100vw;
    margin: 0 auto;
    height: 50px;
    color: #fffefc;
    margin: 0vh auto 0 auto;
    display: flex;
    flex-direction: row;
    align-items: center;
    justify-content: center;
    font-size: 1.5rem;
    font-weight: bold;
    /* font-family: Youshe; */
}

.btnArea {
    /* max-height: 70vh; */
    max-height: calc(92vh - var(--van-action-bar-height) - 50px);
    overflow: scroll;
    margin: 10px auto;
}

.cmdCustomBtn1 {
    margin: 0 auto;
    width: 90vw;
    height: 12vh;
    --van-button-border-width: 0;
    display: flex;
    -webkit-tap-highlight-color: rgba(0, 0, 0, 0);

}

.cmdCustomBtn1 .btn_name {
    line-height: 12vh;
    font-size: 1.5rem;
    font-weight: 600;
    color: #fff;
    margin-left: 30px;
}

.cmdCustomBtn2 {
    width: 90vw;
    line-height: 50px;
    font-size: 1.5rem;
    font-weight: 600;
    color: #fff;
    margin: 10px auto;
}

.cmdResetBtn {
    position: absolute;
    top: 1vh;
    right: 5vw;
    width: 3rem;
    height: 3rem;
}

.arrowdiv {
    position: absolute;
    bottom: 18vh;
    width: 100vw;
    height: 6vh;
    margin: 0 auto;

}

.arrowCol {
    text-align: center;
}

.crtIcon {
    margin-left: auto;
    margin-top: 2vh;
    margin-right: 2vw;
    width: 2rem;
    height: 2rem;
}

.custabbar {
    --van-tabbar-height: 7.5vh
}

.cmdbtn-size {
    background-repeat: no-repeat;
    background-size: cover;
}

.cmdbtn1 {
    background: url(@/assets/img/btn/01-1.jpg);
    background-repeat: no-repeat;
    background-size: cover;
}

.cmdbtn1-1 {
    background: url(@/assets/img/btn/01-1.jpg);
    background-repeat: no-repeat;
    background-size: cover;
}

.cmdbtn1-2 {
    background: url(@/assets/img/btn/01-2.jpg);
    background-repeat: no-repeat;
    background-size: cover;
}

.cmdbtn1-3 {
    background: url(@/assets/img/btn/01-3.jpg);
    background-repeat: no-repeat;
    background-size: cover;
}

.cmdbtn1-4 {
    background: url(@/assets/img/btn/01-4.jpg);
    background-repeat: no-repeat;
    background-size: cover;
}

.cmdbtn2-1 {
    background: url(@/assets/img/btn/02-1.jpg);
    background-repeat: no-repeat;
    background-size: cover;
}

.cmdbtn2-2 {
    background: url(@/assets/img/btn/02-2.jpg);
    background-repeat: no-repeat;
    background-size: cover;
}

.cmdbtn2-3 {
    background: url(@/assets/img/btn/02-3.jpg);
    background-repeat: no-repeat;
    background-size: cover;
}

.cmdbtn2-4 {
    background: url(@/assets/img/btn/02-4.jpg);
    background-repeat: no-repeat;
    background-size: cover;
}

.cmdbtn3-1 {
    background: url(@/assets/img/btn/03-1.jpg);
    background-repeat: no-repeat;
    background-size: cover;
}

.cmdbtn3-2 {
    background: url(@/assets/img/btn/03-2.jpg);
    background-repeat: no-repeat;
    background-size: cover;
}

.cmdbtn3-3 {
    background: url(@/assets/img/btn/03-3.jpg);
    background-repeat: no-repeat;
    background-size: cover;
}

.cmdbtn3-4 {
    background: url(@/assets/img/btn/03-4.jpg);
    background-repeat: no-repeat;
    background-size: cover;
}
</style>

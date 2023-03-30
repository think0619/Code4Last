// article.js
import axios from 'axios'



export function sendCMDMsg(_params) {
    //var host = 'http://10.10.20.221:8080';
    var host = 'http://192.168.18.130:8080';
    var url = host + "/api/HandleMsg/ReceiveMsgFromUser";
    return axios({
        url: url,
        method: "post",
        data: _params
    })
}
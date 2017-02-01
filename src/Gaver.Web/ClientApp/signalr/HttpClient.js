"use strict";
class HttpClient {
    get(url) {
        return this.xhr("GET", url);
    }
    post(url, content) {
        return this.xhr("POST", url, content);
    }
    xhr(method, url, content) {
        return new Promise((resolve, reject) => {
            let xhr = new XMLHttpRequest();
            xhr.open(method, url, true);
            if (method === "POST" && content != null) {
                xhr.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
            }
            xhr.send(content);
            xhr.onload = () => {
                if (xhr.status >= 200 && xhr.status < 300) {
                    resolve(xhr.response);
                }
                else {
                    reject({
                        status: xhr.status,
                        statusText: xhr.statusText
                    });
                }
            };
            xhr.onerror = () => {
                reject({
                    status: xhr.status,
                    statusText: xhr.statusText
                });
            };
        });
    }
}
exports.HttpClient = HttpClient;

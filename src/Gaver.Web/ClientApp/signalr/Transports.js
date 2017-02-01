"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
class WebSocketTransport {
    connect(url, queryString = "") {
        return new Promise((resolve, reject) => {
            url = url.replace(/^http/, "ws");
            let connectUrl = url + "/ws?" + queryString;
            let webSocket = new WebSocket(connectUrl);
            let thisWebSocketTransport = this;
            webSocket.onopen = (event) => {
                console.log(`WebSocket connected to ${connectUrl}`);
                thisWebSocketTransport.webSocket = webSocket;
                resolve();
            };
            webSocket.onerror = (event) => {
                reject();
            };
            webSocket.onmessage = (message) => {
                console.log(`(WebSockets transport) data received: ${message.data}`);
                if (thisWebSocketTransport.onDataReceived) {
                    thisWebSocketTransport.onDataReceived(message.data);
                }
            };
            webSocket.onclose = (event) => {
                // webSocket will be null if the transport did not start successfully
                if (thisWebSocketTransport.webSocket && (event.wasClean === false || event.code !== 1000)) {
                    if (thisWebSocketTransport.onError) {
                        thisWebSocketTransport.onError(event);
                    }
                }
            };
        });
    }
    send(data) {
        if (this.webSocket && this.webSocket.readyState === WebSocket.OPEN) {
            this.webSocket.send(data);
            return Promise.resolve();
        }
        return Promise.reject("WebSocket is not in the OPEN state");
    }
    stop() {
        if (this.webSocket) {
            this.webSocket.close();
            this.webSocket = null;
        }
    }
}
exports.WebSocketTransport = WebSocketTransport;
class ServerSentEventsTransport {
    constructor(httpClient) {
        this.httpClient = httpClient;
    }
    connect(url, queryString) {
        if (typeof (EventSource) === "undefined") {
            Promise.reject("EventSource not supported by the browser.");
        }
        this.queryString = queryString;
        this.url = url;
        let tmp = `${this.url}/sse?${this.queryString}`;
        return new Promise((resolve, reject) => {
            let eventSource = new EventSource(`${this.url}/sse?${this.queryString}`);
            try {
                let thisEventSourceTransport = this;
                eventSource.onmessage = (e) => {
                    if (thisEventSourceTransport.onDataReceived) {
                        thisEventSourceTransport.onDataReceived(e.data);
                    }
                };
                eventSource.onerror = (e) => {
                    reject();
                    // don't report an error if the transport did not start successfully
                    if (thisEventSourceTransport.eventSource && thisEventSourceTransport.onError) {
                        thisEventSourceTransport.onError(e);
                    }
                };
                eventSource.onopen = () => {
                    thisEventSourceTransport.eventSource = eventSource;
                    resolve();
                };
            }
            catch (e) {
                return Promise.reject(e);
            }
        });
    }
    send(data) {
        return __awaiter(this, void 0, void 0, function* () {
            yield this.httpClient.post(this.url + "/send?" + this.queryString, data);
        });
    }
    stop() {
        if (this.eventSource) {
            this.eventSource.close();
            this.eventSource = null;
        }
    }
}
exports.ServerSentEventsTransport = ServerSentEventsTransport;
class LongPollingTransport {
    constructor(httpClient) {
        this.httpClient = httpClient;
    }
    connect(url, queryString) {
        this.url = url;
        this.queryString = queryString;
        this.shouldPoll = true;
        this.poll(url + "/poll?" + this.queryString);
        return Promise.resolve();
    }
    poll(url) {
        if (!this.shouldPoll) {
            return;
        }
        let thisLongPollingTransport = this;
        let pollXhr = new XMLHttpRequest();
        pollXhr.onload = () => {
            if (pollXhr.status == 200) {
                if (thisLongPollingTransport.onDataReceived) {
                    thisLongPollingTransport.onDataReceived(pollXhr.response);
                }
                thisLongPollingTransport.poll(url);
            }
            else if (this.pollXhr.status == 204) {
            }
            else {
                if (thisLongPollingTransport.onError) {
                    thisLongPollingTransport.onError({
                        status: pollXhr.status,
                        statusText: pollXhr.statusText
                    });
                }
            }
        };
        pollXhr.onerror = () => {
            if (thisLongPollingTransport.onError) {
                thisLongPollingTransport.onError({
                    status: pollXhr.status,
                    statusText: pollXhr.statusText
                });
            }
        };
        pollXhr.ontimeout = () => {
            thisLongPollingTransport.poll(url);
        };
        this.pollXhr = pollXhr;
        this.pollXhr.open("GET", url, true);
        // TODO: consider making timeout configurable
        this.pollXhr.timeout = 110000;
        this.pollXhr.send();
    }
    send(data) {
        return __awaiter(this, void 0, void 0, function* () {
            yield this.httpClient.post(this.url + "/send?" + this.queryString, data);
        });
    }
    stop() {
        this.shouldPoll = false;
        if (this.pollXhr) {
            this.pollXhr.abort();
            this.pollXhr = null;
        }
    }
}
exports.LongPollingTransport = LongPollingTransport;

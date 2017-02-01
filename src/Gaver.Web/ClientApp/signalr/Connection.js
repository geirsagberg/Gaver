"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
const Transports_1 = require("./Transports");
const HttpClient_1 = require("./HttpClient");
var ConnectionState;
(function (ConnectionState) {
    ConnectionState[ConnectionState["Disconnected"] = 0] = "Disconnected";
    ConnectionState[ConnectionState["Connecting"] = 1] = "Connecting";
    ConnectionState[ConnectionState["Connected"] = 2] = "Connected";
})(ConnectionState || (ConnectionState = {}));
class Connection {
    constructor(url, queryString = "", options = {}) {
        this.dataReceivedCallback = (data) => { };
        this.connectionClosedCallback = (error) => { };
        this.url = url;
        this.queryString = queryString;
        this.httpClient = options.httpClient || new HttpClient_1.HttpClient();
        this.connectionState = ConnectionState.Disconnected;
    }
    start(transportName = "webSockets") {
        return __awaiter(this, void 0, void 0, function* () {
            if (this.connectionState != ConnectionState.Disconnected) {
                throw new Error("Cannot start a connection that is not in the 'Disconnected' state");
            }
            this.transport = this.createTransport(transportName);
            this.transport.onDataReceived = this.dataReceivedCallback;
            this.transport.onError = e => this.stopConnection(e);
            try {
                this.connectionId = yield this.httpClient.get(`${this.url}/negotiate?${this.queryString}`);
                const queryString = `id=${this.connectionId}&${this.queryString}`;
                yield this.transport.connect(this.url, queryString);
                this.connectionState = ConnectionState.Connected;
            }
            catch (e) {
                console.log("Failed to start the connection.");
                this.connectionState = ConnectionState.Disconnected;
                this.transport = null;
                throw e;
            }
            ;
        });
    }
    createTransport(transportName) {
        if (transportName === "webSockets") {
            return new Transports_1.WebSocketTransport();
        }
        if (transportName === "serverSentEvents") {
            return new Transports_1.ServerSentEventsTransport(this.httpClient);
        }
        if (transportName === "longPolling") {
            return new Transports_1.LongPollingTransport(this.httpClient);
        }
        throw new Error("No valid transports requested.");
    }
    send(data) {
        if (this.connectionState != ConnectionState.Connected) {
            throw new Error("Cannot send data if the connection is not in the 'Connected' State");
        }
        return this.transport.send(data);
    }
    stop() {
        if (this.connectionState != ConnectionState.Connected) {
            throw new Error("Cannot stop the connection if it is not in the 'Connected' State");
        }
        this.stopConnection();
    }
    stopConnection(error) {
        this.transport.stop();
        this.transport = null;
        this.connectionState = ConnectionState.Disconnected;
        this.connectionClosedCallback(error);
    }
    set dataReceived(callback) {
        this.dataReceivedCallback = callback;
    }
    set connectionClosed(callback) {
        this.connectionClosedCallback = callback;
    }
}
exports.Connection = Connection;

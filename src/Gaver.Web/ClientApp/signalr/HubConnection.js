"use strict";
const Connection_1 = require("./Connection");
var Connection_2 = require("./Connection");
exports.Connection = Connection_2.Connection;
class HubConnection {
    constructor(url, queryString) {
        this.connection = new Connection_1.Connection(url, queryString);
        let thisHubConnection = this;
        this.connection.dataReceived = data => {
            thisHubConnection.dataReceived(data);
        };
        this.callbacks = new Map();
        this.methods = new Map();
        this.id = 0;
    }
    dataReceived(data) {
        // TODO: separate JSON parsing
        // Can happen if a poll request was cancelled
        if (!data) {
            return;
        }
        var descriptor = JSON.parse(data);
        if (descriptor.method === undefined) {
            let invocationResult = descriptor;
            let callback = this.callbacks[invocationResult.id];
            if (callback != null) {
                callback(invocationResult);
                this.callbacks.delete(invocationResult.id);
            }
        }
        else {
            let invocation = descriptor;
            let method = this.methods[invocation.method];
            if (method != null) {
                // TODO: bind? args?
                method.apply(this, invocation.arguments);
            }
        }
    }
    start(transportName) {
        return this.connection.start(transportName);
    }
    stop() {
        return this.connection.stop();
    }
    invoke(methodName, ...args) {
        let id = this.id;
        this.id++;
        let invocationDescriptor = {
            "Id": id.toString(),
            "Method": methodName,
            "Arguments": args
        };
        let p = new Promise((resolve, reject) => {
            this.callbacks[id] = (invocationResult) => {
                if (invocationResult.error != null) {
                    reject(new Error(invocationResult.error));
                }
                else {
                    resolve(invocationResult.result);
                }
            };
            //TODO: separate conversion to enable different data formats
            this.connection.send(JSON.stringify(invocationDescriptor))
                .catch(e => {
                // TODO: remove callback
                reject(e);
            });
        });
        return p;
    }
    on(methodName, method) {
        this.methods[methodName] = method;
    }
    set connectionClosed(callback) {
        this.connection.connectionClosed = callback;
    }
}
exports.HubConnection = HubConnection;

import * as signalR from "@microsoft/signalr";
import { store } from "../app/store";
import { addAuditItem, setOnlineCount , setConnected} from "../features/signalR/signalRSlice";
import { applyProjectAudit } from "../features/projects/projectsSlice";

let connection = null;

export function startSignalR() {
  const state = store.getState();
  const token = state.auth.accessToken; 

  if (!token) return;

  connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7122/hubs/company", {
      accessTokenFactory: () => token
    })
    .withAutomaticReconnect()
    .build();

  connection.on("AuditChanged", (data) => {
    store.dispatch(addAuditItem(data));
    store.dispatch(applyProjectAudit(data));
  });

  connection.on("OnlineUsersChanged", (count) => {
    store.dispatch(setOnlineCount(count));
  });

  connection.start().then(() => {
    store.dispatch(setConnected(true));
  });
}

export function stopSignalR() {
  if (connection) {
    connection.stop();
    store.dispatch(setConnected(false));
    connection = null;
  }
}

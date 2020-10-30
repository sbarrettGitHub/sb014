import { HubConnection } from "@aspnet/signalr";
import { getCurrentInstance } from "vue";

export function useHubConnection(): HubConnection| null {
  const root = getCurrentInstance();
  let hubConnection: HubConnection | null = null;
  if (root) {
    hubConnection = root.appContext.config.globalProperties.connection();
  }

  return hubConnection;
}

import { HubConnectionBuilder, LogLevel } from "@aspnet/signalr";
export default {
  // eslint-disable-next-line
  install(Vue: any) {
    const connection = new HubConnectionBuilder()
      .withUrl(process.env.VUE_APP_API_BASE_URL + "/TournamentHub")
      .configureLogging(LogLevel.Information)
      .build();
    let reconnection: NodeJS.Timeout;

    async function reconnect() {
      try {
        await connection.start();
        clearInterval(reconnection);
      } catch (error) {
        console.log("Reconnect failed ... trying again in 5 seconds");
      }
    }
    connection.onclose(function() {
      setTimeout(async function() {
        console.log("Connection closed ... reconnecting after 5 seconds");
        try {
          await connection.start();
        } catch {
          reconnection = setInterval(reconnect, 5000);
        }
      }, 5000); // Restart connection after 5 seconds.
    });

    connection.start();

    Vue.config.globalProperties.connection = () => {
      return connection;
    };
  },
};

import { createAppContainer, createSwitchNavigator } from "react-navigation";
import AuthNavigator from "./views/navigator/AuthNavigator";
import AdminApp from "./AdminPanel";
import UserApp from "./UserPanel";

const SwitchNavigator = createSwitchNavigator(
  {
    Auth: {
      screen: AuthNavigator
    },
    AdminPanel: {
      screen: AdminApp
    },
    UserPanel: {
      screen: UserApp
    }
  },
  {
    initialRouteName: "Auth",
    defaultNavigationOptions: "Auth"
  }
);

const AppContainer = createAppContainer(SwitchNavigator);
export default AppContainer;

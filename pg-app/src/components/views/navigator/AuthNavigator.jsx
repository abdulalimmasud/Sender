import { createStackNavigator } from "react-navigation-stack";
import Login from "../Login";

const AuthNavigator = createStackNavigator(
  {
    Login: { screen: Login }
  },
  {
    initialRouteName: "Login"
  }
);

export default AuthNavigator;

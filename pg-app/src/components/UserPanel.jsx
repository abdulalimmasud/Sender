import { Ionicons, AntDesign } from "@expo/vector-icons";
import React from "react";
import { createDrawerNavigator } from "react-navigation-drawer";
import { createAppContainer } from "react-navigation";
import CustomDrawerNavigator from "./views/navigator/CustomDrawerNavigator";
import Dashboard from "./views/Dashboard";
import UserReport from "./views/UserReport";
import Logout from "./views/Logout";

const UserNavigator = createDrawerNavigator(
  {
    UserDashboard: {
      navigationOptions: {
        drawerIcon: ({ tintColor }) => (
          <Ionicons name="md-home" style={{ color: tintColor }} />
        ),
        drawerLabel: "Monitoring",
        title: "Monitoring"
      },
      screen: Dashboard
    },
    UserReport: {
      navigationOptions: {
        drawerIcon: ({ tintColor }) => (
          <Ionicons name="md-book" style={{ color: tintColor }} />
        ),
        drawerLabel: "Report",
        title: "Report"
      },
      screen: UserReport
    },
    Logout: {
      navigationOptions: {
        drawerIcon: ({ tintColor }) => (
          <AntDesign name="logout" style={{ color: tintColor }} />
        ),
        drawerLabel: "Logout"
      },
      screen: Logout
    }
  },
  {
    contentComponent: CustomDrawerNavigator,
    initialRouteName: "UserDashboard"
  }
);

const UserApp = createAppContainer(UserNavigator);

export default UserApp;

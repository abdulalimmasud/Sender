import { Ionicons, AntDesign } from "@expo/vector-icons";
import React from "react";
import { createDrawerNavigator } from "react-navigation-drawer";
import { createAppContainer } from "react-navigation";
import CustomDrawerNavigator from "./views/navigator/CustomDrawerNavigator";
import Dashboard from "./views/Dashboard";
import AdminReport from "./views/AdminReport";
import Logout from "./views/Logout";

const AdminNavigator = createDrawerNavigator(
  {
    Dashboard: {
      navigationOptions: {
        drawerIcon: ({ tintColor }) => (
          <Ionicons name="md-home" style={{ color: tintColor }} />
        ),
        drawerLabel: "Monitoring",
        title: "Monitoring"
      },
      screen: Dashboard
    },
    AdminReport: {
      navigationOptions: {
        drawerIcon: ({ tintColor }) => (
          <Ionicons name="md-book" style={{ color: tintColor }} />
        ),
        drawerLabel: "Report",
        title: "Report"
      },
      screen: AdminReport
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
    style: { marginTop: 50 }
  }
);

const AdminApp = createAppContainer(AdminNavigator);

export default AdminApp;

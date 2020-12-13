import { createMaterialTopTabNavigator } from "react-navigation-tabs";
import CustomReport from "../report/CustomReport";
import SiteReport from "../report/SiteReport";

const UserReportNavigator = createMaterialTopTabNavigator(
  {
    SiteReport: {
      navigationOptions: {
        tabBarLabel: "Site Report"
      },
      screen: SiteReport
    },
    CustomReport: {
      navigationOptions: {
        tabBarLabel: "Custom Report"
      },
      screen: CustomReport
    }
  },
  {
    tabBarPosition: "bottom",
    tabBarOptions: {
      style: {
        backgroundColor: "#ad9898"
      }
    }
  }
);
UserReportNavigator.navigationOptions = ({ navigation }) => ({
  tabBarVisible: navigation.state.index === 0,
  swipeEnabled: navigation.state.index === 0
});
export default UserReportNavigator;

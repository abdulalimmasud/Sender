import { createMaterialTopTabNavigator } from "react-navigation-tabs";
import CustomReport from "../report/CustomReport";
import MontlyReport from "../report/MontlyReport";
import SiteReport from "../report/SiteReport";

const ReportNavigator = createMaterialTopTabNavigator(
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
    },
    MontlyReport: {
      navigationOptions: {
        tabBarLabel: "Monthly Report"
      },
      screen: MontlyReport
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
ReportNavigator.navigationOptions = ({ navigation }) => ({
  tabBarVisible: navigation.state.index === 0,
  swipeEnabled: navigation.state.index === 0
});
export default ReportNavigator;

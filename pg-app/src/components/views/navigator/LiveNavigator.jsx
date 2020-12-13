import { createMaterialTopTabNavigator } from "react-navigation-tabs";
import Monitoring from "../live/Monitoring";
import LiveStatus from "../live/LiveStatus";
import OnMap from "../live/OnMap";

const LiveNavigator = createMaterialTopTabNavigator(
  {
    Monitoring: {
      navigationOptions: {
        tabBarLabel: "PG On Sites"
      },
      screen: Monitoring
    },
    LiveStatus: {
      navigationOptions: {
        tabBarLabel: "PG Live Status"
      },
      screen: LiveStatus
    },
    OnMap: {
      navigationOptions: {
        tabBarLabel: "PG On Map"
      },
      screen: OnMap
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

LiveNavigator.navigationOptions = ({ navigation }) => ({
  tabBarVisible: navigation.state.index === 0,
  swipeEnabled: navigation.state.index === 0
});

export default LiveNavigator;

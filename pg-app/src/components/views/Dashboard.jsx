import React, { Component } from "react";
import { View, Text, StyleSheet } from "react-native";
import Container from "../common/Container";
import Colors from "../utils/colors";
import LiveNavigator from "./navigator/LiveNavigator";
import CustomHeader from "../common/CustomHeader";

class Dashboard extends Component {
  static router = LiveNavigator.router;
  render() {
    return (
      <Container>
        <CustomHeader navigation={this.props.navigation} />
        <LiveNavigator navigation={this.props.navigation} />
        {/* <View style={styles.container}>
          
        </View> */}
      </Container>
    );
  }
}
const styles = StyleSheet.create({
  container: {
    backgroundColor: Colors.lightBlue,
    alignItems: "center",
    justifyContent: "center"
  }
});

export default Dashboard;

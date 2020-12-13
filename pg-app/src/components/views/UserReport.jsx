import React, { Component } from "react";
import { View, Text, StyleSheet } from "react-native";
import Container from "../common/Container";
import Colors from "../utils/colors";
import UserReportNavigator from "./navigator/UserReportNavigator";
import CustomHeader from "../common/CustomHeader";

class UserReport extends Component {
  static router = UserReportNavigator.router;
  render() {
    return (
      <Container>
        <CustomHeader navigation={this.props.navigation} />
        <UserReportNavigator navigation={this.props.navigation} />
        {/* <View style={styles.container}>
          
        </View> */}
      </Container>
    );
  }
}
const styles = StyleSheet.create({
  container: {
    backgroundColor: "black",
    alignItems: "center",
    justifyContent: "center",
    padding: 50
  }
});

export default UserReport;
